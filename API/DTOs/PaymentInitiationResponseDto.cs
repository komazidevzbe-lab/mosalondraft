namespace API.DTOs;

public class PaymentInitiationResponseDto
{
    public int BookingId { get; set; }

    public string BookingReference { get; set; } = string.Empty;

    public string PaymentProvider { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string PaymentUrl { get; set; } = string.Empty;

    public Dictionary<string, string> FormFields { get; set; } = [];
}