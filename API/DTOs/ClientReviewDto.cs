namespace API.DTOs;

public class ClientReviewDto
{
    public int Id { get; set; }

    public string ClientName { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string ReviewText { get; set; } = string.Empty;

    public int Rating { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public string AltText { get; set; } = string.Empty;

    public bool IsApproved { get; set; }

    public bool IsFeatured { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; }
}