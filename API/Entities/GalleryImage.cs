namespace API.Entities;

public class GalleryImage
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public required string Category { get; set; }

    public required string ImageUrl { get; set; }

    public required string AltText { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public ICollection<GalleryImageFavorite> Favorites { get; set; } = new List<GalleryImageFavorite>();
}