namespace API.DTOs;

public class CreateBookingDto
{
    public string BookingMode { get; set; } = string.Empty;

    public string ClientFullName { get; set; } = string.Empty;

    public string ClientEmailAddress { get; set; } = string.Empty;

    public string ClientPhoneNumber { get; set; } = string.Empty;

    public string PreferredContactMethod { get; set; } = string.Empty;

    public IReadOnlyList<CreateBookingItemDto> Items { get; set; } = [];
}