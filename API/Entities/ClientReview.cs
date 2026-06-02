namespace API.Entities;

public class ClientReview
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public AppUser? User { get; set; }

    public required string ClientName { get; set; }

    public required string Location { get; set; }

    public required string ReviewText { get; set; }

    public int Rating { get; set; }

    public required string ImageUrl { get; set; }

    public required string AltText { get; set; }

    public bool IsApproved { get; set; } = false;

    public bool IsFeatured { get; set; } = false;

    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}