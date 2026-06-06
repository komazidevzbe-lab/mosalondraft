namespace API.Entities;

public class Booking
{
    public int Id { get; set; }

    public required string BookingReference { get; set; }

    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public required string BookingMode { get; set; }

    public required string ClientFullName { get; set; }
    public required string ClientEmailAddress { get; set; }
    public required string ClientPhoneNumber { get; set; }
    public required string PreferredContactMethod { get; set; }

    public int TotalDurationMinutes { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal DepositAmount { get; set; }

    public decimal BalanceAmount { get; set; }

    public required string BookingStatus { get; set; }

    public required string PaymentStatus { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ConfirmedAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public ICollection<BookingServiceItem> Items { get; set; } = new List<BookingServiceItem>();

    public ICollection<BookingPayment> Payments { get; set; } = new List<BookingPayment>();
}