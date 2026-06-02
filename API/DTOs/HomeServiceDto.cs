namespace API.DTOs;

public class HomeServiceDto
{
    public int Id { get; set; }

    public string Slug { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public string AltText { get; set; } = string.Empty;

    public string IconUrl { get; set; } = string.Empty;

    public string IconAltText { get; set; } = string.Empty;

    public int DurationMinutes { get; set; }

    public decimal BasePrice { get; set; }

    public int DisplayOrder { get; set; }
}