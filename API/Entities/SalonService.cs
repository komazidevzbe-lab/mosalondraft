namespace API.Entities;

public class SalonService
{
    public int Id { get; set; }

    public required string Slug { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public required string ImageUrl { get; set; }

    public required string AltText { get; set; }

    public required string IconUrl { get; set; }

    public required string IconAltText { get; set; }

    public int DurationMinutes { get; set; }

    public decimal BasePrice { get; set; }

    public bool IsFeaturedOnHome { get; set; } = false;

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // ===============================
    // Service type relationship
    // Example: Gel Manicure, Acrylic Set, Soft Glam, Volume Lashes.
    // ===============================

    public ICollection<SalonServiceType> ServiceTypes { get; set; } = new List<SalonServiceType>();

    // ===============================
    // Service length option relationship
    // Example: Short, Medium, Long, Extra Long.
    // Only services that require length selection should have length options.
    // ===============================

    public ICollection<SalonServiceLengthOption> LengthOptions { get; set; } = new List<SalonServiceLengthOption>();

    // ===============================
    // Booking item relationship
    // Stores every booking item that used this salon service.
    // This keeps old bookings stable even if the service is edited later.
    // ===============================

    public ICollection<BookingServiceItem> BookingItems { get; set; } = new List<BookingServiceItem>();
}