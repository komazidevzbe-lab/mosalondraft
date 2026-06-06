namespace API.DTOs;

public class BookingReviewItemDto
{
    public int Id { get; set; }

    public int SalonServiceId { get; set; }

    public string ServiceName { get; set; } = string.Empty;

    public string ServiceTypeName { get; set; } = string.Empty;

    public string? LengthName { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int DurationMinutes { get; set; }

    public decimal BasePrice { get; set; }

    public decimal LengthAddOnPrice { get; set; }

    public decimal FinalPrice { get; set; }

    public string? Notes { get; set; }

    public string? ReferenceImageUrl { get; set; }
}