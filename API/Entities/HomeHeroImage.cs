namespace API.Entities;

public class HomeHeroImage
{
    public int Id { get; set; }

    public required string Category { get; set; }

    public required string ImageUrl { get; set; }

    public required string AltText { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}