namespace API.Entities;

public class BookingPayment
{
    public int Id { get; set; }

    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;

    public required string PaymentProvider { get; set; }

    public required string PaymentStatus { get; set; }

    public decimal Amount { get; set; }

    public string? MerchantReference { get; set; }

    public string? GatewayPaymentId { get; set; }

    public string? GatewayTransactionId { get; set; }

    public string? RawGatewayResponse { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? PaidAt { get; set; }

    public DateTime? FailedAt { get; set; }

    public DateTime? CancelledAt { get; set; }
}