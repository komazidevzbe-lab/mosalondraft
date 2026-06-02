namespace API.DTOs;

public class HomeReviewDto
{
    public int Id { get; set; }

    public string ClientName { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string ReviewText { get; set; } = string.Empty;

    public int Rating { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public string AltText { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }
}