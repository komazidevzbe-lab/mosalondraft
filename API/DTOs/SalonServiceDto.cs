namespace API.DTOs;

public class SalonServiceDto
{
    public int Id { get; set; }

    public string Slug { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public string AltText { get; set; } = string.Empty;

    public int DurationMinutes { get; set; }

    public decimal BasePrice { get; set; }

    public bool RequiresLength { get; set; }

    public int DisplayOrder { get; set; }

    public IReadOnlyList<SalonServiceTypeDto> ServiceTypes { get; set; } = [];

    public IReadOnlyList<SalonServiceLengthOptionDto> LengthOptions { get; set; } = [];
}