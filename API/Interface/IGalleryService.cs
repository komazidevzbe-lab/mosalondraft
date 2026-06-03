using API.DTOs;

namespace API.Interfaces;

public interface IGalleryService
{
    Task<IReadOnlyList<GalleryImageDto>> GetActiveGalleryImagesAsync(string? category, int userId);

    Task<IReadOnlyList<GalleryImageDto>> GetFavoriteGalleryImagesAsync(int userId);

    Task<GalleryImageDto> AddGalleryImageToFavoritesAsync(int galleryImageId, int userId);

    Task<GalleryImageDto> RemoveGalleryImageFromFavoritesAsync(int galleryImageId, int userId);
}