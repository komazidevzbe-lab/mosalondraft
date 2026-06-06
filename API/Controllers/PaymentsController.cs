using API.DTOs;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class PaymentsController(
    IPaymentService paymentService,
    ILogger<PaymentsController> logger
) : BaseApiController
{
    [HttpPost("payfast/initiate")]
    public async Task<ActionResult<PaymentInitiationResponseDto>> InitiatePayFastPayment(
        InitiatePaymentDto initiatePaymentDto)
    {
        try
        {
            var userId = User.GetUserId();

            var response = await paymentService.InitiateDepositPaymentAsync(
                initiatePaymentDto.BookingId,
                userId);

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("payfast/notify")]
    public async Task<IActionResult> PayFastNotify()
    {
        try
        {
            if (!Request.HasFormContentType)
            {
                logger.LogWarning("PayFast notify request was not form content.");

                return BadRequest(new { message = "Invalid PayFast notification format." });
            }

            var payFastForm = await Request.ReadFormAsync();

            var notificationProcessed = await paymentService.ProcessPayFastNotificationAsync(payFastForm);

            if (!notificationProcessed)
            {
                return BadRequest(new { message = "PayFast notification could not be validated." });
            }

            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the PayFast notify request.");

            return BadRequest(new { message = "PayFast notification could not be processed." });
        }
    }

    [Authorize]
    [HttpPost("payfast/cancel/{bookingId:int}")]
    public async Task<ActionResult> CancelPayFastPayment(int bookingId)
    {
        var userId = User.GetUserId();

        var cancelled = await paymentService.MarkPaymentCancelledAsync(bookingId, userId);

        if (!cancelled)
        {
            return NotFound(new { message = "Booking not found." });
        }

        return Ok(new { message = "Payment cancelled." });
    }
}