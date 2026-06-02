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
}