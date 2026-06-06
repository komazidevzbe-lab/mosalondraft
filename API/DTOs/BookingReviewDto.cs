namespace API.DTOs;

public class BookingReviewDto
{
    public int BookingId { get; set; }

    public string BookingReference { get; set; } = string.Empty;

    public string BookingMode { get; set; } = string.Empty;

    public string BookingStatus { get; set; } = string.Empty;

    public string PaymentStatus { get; set; } = string.Empty;

    public string ClientFullName { get; set; } = string.Empty;

    public string ClientEmailAddress { get; set; } = string.Empty;

    public string ClientPhoneNumber { get; set; } = string.Empty;

    public string PreferredContactMethod { get; set; } = string.Empty;

    public int TotalDurationMinutes { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal DepositAmount { get; set; }

    public decimal BalanceAmount { get; set; }

    public IReadOnlyList<BookingReviewItemDto> Items { get; set; } = [];
}