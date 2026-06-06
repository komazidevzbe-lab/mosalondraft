namespace API.Entities;

public class BookingServiceItem
{
    public int Id { get; set; }

    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;

    public int SalonServiceId { get; set; }
    public SalonService SalonService { get; set; } = null!;

    public int SalonServiceTypeId { get; set; }
    public SalonServiceType SalonServiceType { get; set; } = null!;

    public int? SalonServiceLengthOptionId { get; set; }
    public SalonServiceLengthOption? SalonServiceLengthOption { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public required string ServiceNameSnapshot { get; set; }

    public required string ServiceTypeNameSnapshot { get; set; }

    public string? LengthNameSnapshot { get; set; }

    public decimal BasePriceSnapshot { get; set; }

    public decimal LengthAddOnPriceSnapshot { get; set; }

    public decimal FinalPrice { get; set; }

    public int DurationMinutesSnapshot { get; set; }

    public string? Notes { get; set; }

    public string? ReferenceImageType { get; set; }

    public int? GalleryImageId { get; set; }
    public GalleryImage? GalleryImage { get; set; }

    public string? UploadedReferenceImageUrl { get; set; }

    public string? ReferenceImagePreviewUrl { get; set; }
}