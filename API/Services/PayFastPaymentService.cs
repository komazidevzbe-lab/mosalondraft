using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Services;

public class PayFastPaymentService(
    DataContext context,
    IOptions<PayFastSettings> payFastOptions,
    ILogger<PayFastPaymentService> logger
) : IPaymentService
{
    private readonly PayFastSettings payFastSettings = payFastOptions.Value;

    public async Task<PaymentInitiationResponseDto> InitiateDepositPaymentAsync(
        int bookingId,
        int userId)
    {
        var booking = await context.Bookings
            .Include(booking => booking.Payments)
            .SingleOrDefaultAsync(booking =>
                booking.Id == bookingId &&
                booking.UserId == userId);

        if (booking == null)
        {
            throw new KeyNotFoundException("Booking not found.");
        }

        if (booking.BookingStatus != "PendingPayment")
        {
            throw new ArgumentException("Only pending bookings can be paid.");
        }

        if (booking.PaymentStatus == "Paid")
        {
            throw new ArgumentException("This booking has already been paid.");
        }

        ValidatePayFastSettings();

        var merchantReference = booking.BookingReference;
        var returnUrl = BuildUrlWithBookingId(payFastSettings.ReturnUrl, booking.Id);
        var cancelUrl = BuildUrlWithBookingId(payFastSettings.CancelUrl, booking.Id);
        var clientNames = SplitClientName(booking.ClientFullName);

        var formFields = new Dictionary<string, string>();

        AddField(formFields, "merchant_id", payFastSettings.MerchantId);
        AddField(formFields, "merchant_key", payFastSettings.MerchantKey);
        AddField(formFields, "return_url", returnUrl);
        AddField(formFields, "cancel_url", cancelUrl);
        AddField(formFields, "notify_url", payFastSettings.NotifyUrl);

        AddField(formFields, "name_first", clientNames.FirstName);
        AddField(formFields, "name_last", clientNames.LastName);
        AddField(formFields, "email_address", booking.ClientEmailAddress);
        AddField(formFields, "cell_number", NormaliseCellNumber(booking.ClientPhoneNumber));

        AddField(formFields, "m_payment_id", merchantReference);
        AddField(
            formFields,
            "amount",
            booking.DepositAmount.ToString("0.00", CultureInfo.InvariantCulture));
        AddField(formFields, "item_name", $"Booking Deposit {booking.BookingReference}");
        AddField(formFields, "item_description", "Deposit payment for salon service booking");

        var signature = GenerateSignature(formFields, payFastSettings.Passphrase);

        formFields.Add("signature", signature);

        var existingPendingPayment = booking.Payments
            .OrderByDescending(payment => payment.CreatedAt)
            .FirstOrDefault(payment =>
                payment.PaymentProvider == "PayFast" &&
                payment.MerchantReference == merchantReference &&
                payment.PaymentStatus == "Pending");

        if (existingPendingPayment == null)
        {
            var payment = new BookingPayment
            {
                BookingId = booking.Id,
                PaymentProvider = "PayFast",
                PaymentStatus = "Pending",
                Amount = booking.DepositAmount,
                MerchantReference = merchantReference,
                CreatedAt = DateTime.UtcNow
            };

            context.BookingPayments.Add(payment);
        }

        booking.PaymentStatus = "PendingPayment";

        await context.SaveChangesAsync();

        return new PaymentInitiationResponseDto
        {
            BookingId = booking.Id,
            BookingReference = booking.BookingReference,
            PaymentProvider = "PayFast",
            Amount = booking.DepositAmount,
            PaymentUrl = payFastSettings.ProcessUrl.Trim(),
            FormFields = formFields
        };
    }

    public async Task<bool> MarkPaymentCancelledAsync(int bookingId, int userId)
    {
        var booking = await context.Bookings
            .Include(booking => booking.Payments)
            .SingleOrDefaultAsync(booking =>
                booking.Id == bookingId &&
                booking.UserId == userId);

        if (booking == null)
        {
            return false;
        }

        booking.PaymentStatus = "Cancelled";
        booking.CancelledAt ??= DateTime.UtcNow;

        var latestPayment = booking.Payments
            .OrderByDescending(payment => payment.CreatedAt)
            .FirstOrDefault();

        if (latestPayment != null)
        {
            latestPayment.PaymentStatus = "Cancelled";
            latestPayment.CancelledAt ??= DateTime.UtcNow;
        }

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ProcessPayFastNotificationAsync(IFormCollection payFastForm)
    {
        ValidatePayFastSettings();

        if (payFastForm.Count == 0)
        {
            logger.LogWarning("PayFast notification received with no form fields.");
            return false;
        }

        var formFields = ConvertFormToDictionary(payFastForm);

        LogPayFastFieldDebugInfo(formFields);

        var signatureIsValid = ValidatePayFastNotificationSignature(formFields);

        if (!signatureIsValid)
        {
            logger.LogWarning("PayFast notification signature validation failed.");

            if (!payFastSettings.SandboxMode)
            {
                return false;
            }

            logger.LogWarning("SandboxMode is enabled, so the PayFast notification will continue after signature failure for local testing.");
        }

        var merchantId = GetFormValue(formFields, "merchant_id");

        if (!merchantId.Equals(payFastSettings.MerchantId.Trim(), StringComparison.Ordinal))
        {
            logger.LogWarning(
                "PayFast merchant ID mismatch. Expected {ExpectedMerchantId}, received {ReceivedMerchantId}.",
                payFastSettings.MerchantId.Trim(),
                merchantId);

            return false;
        }

        var merchantReference = GetFormValue(formFields, "m_payment_id");

        if (string.IsNullOrWhiteSpace(merchantReference))
        {
            logger.LogWarning("PayFast notification is missing m_payment_id.");
            return false;
        }

        var booking = await context.Bookings
            .Include(booking => booking.Payments)
            .SingleOrDefaultAsync(booking => booking.BookingReference == merchantReference);

        if (booking == null)
        {
            logger.LogWarning(
                "PayFast notification received for unknown booking reference {BookingReference}.",
                merchantReference);

            return false;
        }

        var amountValue = GetFirstAvailableFormValue(
            formFields,
            "amount_gross",
            "amount",
            "amount_net");

        if (!TryParsePayFastAmount(amountValue, out var paidAmount))
        {
            logger.LogWarning(
                "PayFast notification has an invalid amount value. Received amount: {AmountValue}.",
                amountValue);

            return false;
        }

        if (Math.Round(paidAmount, 2) != Math.Round(booking.DepositAmount, 2))
        {
            logger.LogWarning(
                "PayFast amount mismatch for booking {BookingReference}. Expected {ExpectedAmount}, received {ReceivedAmount}.",
                booking.BookingReference,
                booking.DepositAmount,
                paidAmount);

            return false;
        }

        var payFastPaymentStatus = GetFormValue(formFields, "payment_status");

        if (string.IsNullOrWhiteSpace(payFastPaymentStatus))
        {
            logger.LogWarning("PayFast notification is missing payment_status.");
            return false;
        }

        var payFastPaymentId = GetFormValue(formFields, "pf_payment_id");
        var rawGatewayResponse = BuildRawGatewayResponse(formFields);

        var payment = booking.Payments
            .OrderByDescending(payment => payment.CreatedAt)
            .FirstOrDefault(payment =>
                payment.PaymentProvider == "PayFast" &&
                payment.MerchantReference == merchantReference);

        if (payment == null)
        {
            payment = new BookingPayment
            {
                BookingId = booking.Id,
                PaymentProvider = "PayFast",
                PaymentStatus = "Pending",
                Amount = booking.DepositAmount,
                MerchantReference = merchantReference,
                CreatedAt = DateTime.UtcNow
            };

            context.BookingPayments.Add(payment);
        }

        if (!string.IsNullOrWhiteSpace(payFastPaymentId))
        {
            payment.GatewayPaymentId = payFastPaymentId;
            payment.GatewayTransactionId = payFastPaymentId;
        }

        payment.RawGatewayResponse = rawGatewayResponse;

        if (payFastPaymentStatus.Equals("COMPLETE", StringComparison.OrdinalIgnoreCase))
        {
            booking.PaymentStatus = "Paid";
            booking.BookingStatus = "Confirmed";
            booking.ConfirmedAt ??= DateTime.UtcNow;
            booking.CancelledAt = null;

            payment.PaymentStatus = "Paid";
            payment.PaidAt ??= DateTime.UtcNow;
            payment.FailedAt = null;
            payment.CancelledAt = null;
        }
        else if (payFastPaymentStatus.Equals("FAILED", StringComparison.OrdinalIgnoreCase))
        {
            booking.PaymentStatus = "Failed";

            payment.PaymentStatus = "Failed";
            payment.FailedAt ??= DateTime.UtcNow;
        }
        else if (payFastPaymentStatus.Equals("CANCELLED", StringComparison.OrdinalIgnoreCase))
        {
            booking.PaymentStatus = "Cancelled";
            booking.CancelledAt ??= DateTime.UtcNow;

            payment.PaymentStatus = "Cancelled";
            payment.CancelledAt ??= DateTime.UtcNow;
        }
        else
        {
            payment.PaymentStatus = payFastPaymentStatus;
        }

        await context.SaveChangesAsync();

        logger.LogInformation(
            "PayFast notification processed for booking {BookingReference}. BookingStatus: {BookingStatus}. PaymentStatus: {PaymentStatus}.",
            booking.BookingReference,
            booking.BookingStatus,
            booking.PaymentStatus);

        return true;
    }

    private void ValidatePayFastSettings()
    {
        if (string.IsNullOrWhiteSpace(payFastSettings.MerchantId))
        {
            throw new InvalidOperationException("PayFast MerchantId is missing.");
        }

        if (string.IsNullOrWhiteSpace(payFastSettings.MerchantKey))
        {
            throw new InvalidOperationException("PayFast MerchantKey is missing.");
        }

        if (string.IsNullOrWhiteSpace(payFastSettings.Passphrase))
        {
            throw new InvalidOperationException("PayFast Passphrase is missing.");
        }

        if (string.IsNullOrWhiteSpace(payFastSettings.ProcessUrl))
        {
            throw new InvalidOperationException("PayFast ProcessUrl is missing.");
        }

        if (string.IsNullOrWhiteSpace(payFastSettings.ReturnUrl))
        {
            throw new InvalidOperationException("PayFast ReturnUrl is missing.");
        }

        if (string.IsNullOrWhiteSpace(payFastSettings.CancelUrl))
        {
            throw new InvalidOperationException("PayFast CancelUrl is missing.");
        }

        if (string.IsNullOrWhiteSpace(payFastSettings.NotifyUrl))
        {
            throw new InvalidOperationException("PayFast NotifyUrl is missing.");
        }

        if (payFastSettings.NotifyUrl.Contains("your-public-api-url", StringComparison.OrdinalIgnoreCase) ||
            payFastSettings.NotifyUrl.Contains("YOUR-NGROK-URL", StringComparison.OrdinalIgnoreCase) ||
            payFastSettings.NotifyUrl.Contains("YOUR-REAL-NGROK-URL", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("PayFast NotifyUrl is still a placeholder. Replace it with your public ngrok API URL.");
        }
    }

    private static void AddField(
        IDictionary<string, string> formFields,
        string key,
        string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        formFields.Add(key, value.Trim());
    }

    private static string BuildUrlWithBookingId(string baseUrl, int bookingId)
    {
        var trimmedBaseUrl = baseUrl.Trim();
        var separator = trimmedBaseUrl.Contains('?') ? "&" : "?";

        return $"{trimmedBaseUrl}{separator}bookingId={bookingId}";
    }

    private static Dictionary<string, string> ConvertFormToDictionary(IFormCollection form)
    {
        var formFields = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var key in form.Keys)
        {
            var value = form[key].ToString();

            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            formFields[key] = value.Trim();
        }

        return formFields;
    }

    private bool ValidatePayFastNotificationSignature(IReadOnlyDictionary<string, string> formFields)
    {
        var submittedSignature = GetFormValue(formFields, "signature");

        if (string.IsNullOrWhiteSpace(submittedSignature))
        {
            return false;
        }

        var generatedSignature = GenerateSignature(formFields, payFastSettings.Passphrase);

        if (!submittedSignature.Equals(generatedSignature, StringComparison.OrdinalIgnoreCase))
        {
            logger.LogWarning(
                "PayFast signature mismatch. Submitted: {SubmittedSignature}. Generated: {GeneratedSignature}.",
                submittedSignature,
                generatedSignature);

            return false;
        }

        return true;
    }

    private static string GenerateSignature(
        IReadOnlyDictionary<string, string> formFields,
        string passphrase)
    {
        var parameterString = string.Join(
            "&",
            formFields
                .Where(field =>
                    !field.Key.Equals("signature", StringComparison.OrdinalIgnoreCase) &&
                    !string.IsNullOrWhiteSpace(field.Value))
                .Select(field =>
                    $"{field.Key}={EncodePayFastValue(field.Value)}"));

        if (!string.IsNullOrWhiteSpace(passphrase))
        {
            parameterString += $"&passphrase={EncodePayFastValue(passphrase.Trim())}";
        }

        using var md5 = MD5.Create();

        var inputBytes = Encoding.UTF8.GetBytes(parameterString);
        var hashBytes = md5.ComputeHash(inputBytes);

        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }

    private static string EncodePayFastValue(string value)
    {
        var encodedValue = WebUtility.UrlEncode(value.Trim()) ?? string.Empty;

        return Regex.Replace(
            encodedValue,
            "%[0-9a-f]{2}",
            match => match.Value.ToUpperInvariant());
    }

    private static string GetFormValue(
        IReadOnlyDictionary<string, string> formFields,
        string key)
    {
        return formFields.TryGetValue(key, out var value)
            ? value.Trim()
            : string.Empty;
    }

    private static string GetFirstAvailableFormValue(
        IReadOnlyDictionary<string, string> formFields,
        params string[] keys)
    {
        foreach (var key in keys)
        {
            var value = GetFormValue(formFields, key);

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        return string.Empty;
    }

    private static bool TryParsePayFastAmount(string amount, out decimal parsedAmount)
    {
        return decimal.TryParse(
            amount,
            NumberStyles.Number,
            CultureInfo.InvariantCulture,
            out parsedAmount);
    }

    private static string BuildRawGatewayResponse(IReadOnlyDictionary<string, string> formFields)
    {
        var rawGatewayResponse = string.Join(
            "&",
            formFields.Select(field => $"{field.Key}={field.Value}"));

        return rawGatewayResponse.Length <= 4000
            ? rawGatewayResponse
            : rawGatewayResponse[..4000];
    }

    private void LogPayFastFieldDebugInfo(IReadOnlyDictionary<string, string> formFields)
    {
        logger.LogInformation(
            "PayFast ITN received. MerchantId: {MerchantId}. BookingReference: {BookingReference}. PaymentStatus: {PaymentStatus}. AmountGross: {AmountGross}. Amount: {Amount}.",
            GetFormValue(formFields, "merchant_id"),
            GetFormValue(formFields, "m_payment_id"),
            GetFormValue(formFields, "payment_status"),
            GetFormValue(formFields, "amount_gross"),
            GetFormValue(formFields, "amount"));
    }

    private static (string FirstName, string LastName) SplitClientName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            return (string.Empty, string.Empty);
        }

        var nameParts = fullName
            .Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (nameParts.Length == 1)
        {
            return (nameParts[0], string.Empty);
        }

        var firstName = nameParts[0];
        var lastName = string.Join(" ", nameParts.Skip(1));

        return (firstName, lastName);
    }

    private static string NormaliseCellNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return string.Empty;
        }

        return Regex.Replace(phoneNumber.Trim(), @"\s+", string.Empty);
    }
}