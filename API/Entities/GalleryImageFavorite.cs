namespace API.Entities;

public class GalleryImageFavorite
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public AppUser User { get; set; } = null!;

    public int GalleryImageId { get; set; }

    public GalleryImage GalleryImage { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}