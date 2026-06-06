namespace API.DTOs;

public class CreateBookingItemDto
{
    public int SalonServiceId { get; set; }

    public int SalonServiceTypeId { get; set; }

    public int? SalonServiceLengthOptionId { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public string? Notes { get; set; }

    public int? GalleryImageId { get; set; }

    public string? UploadedReferenceImageUrl { get; set; }
}