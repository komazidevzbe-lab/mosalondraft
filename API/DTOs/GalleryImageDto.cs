namespace API.DTOs;

public class GalleryImageDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public string AltText { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public bool IsFavorite { get; set; }
}