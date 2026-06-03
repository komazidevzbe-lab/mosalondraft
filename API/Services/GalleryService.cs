using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class GalleryService(
    DataContext context,
    IMapper mapper
) : IGalleryService
{
    private static readonly string[] ValidCategories =
    [
        "manicure",
        "pedicure",
        "makeup",
        "lashes"
    ];

    public async Task<IReadOnlyList<GalleryImageDto>> GetActiveGalleryImagesAsync(
        string? category,
        int userId)
    {
        var cleanedCategory = category?.Trim().ToLowerInvariant();

        ValidateCategory(cleanedCategory);

        var favoriteImageIds = await context.GalleryImageFavorites
            .AsNoTracking()
            .Where(favorite => favorite.UserId == userId)
            .Select(favorite => favorite.GalleryImageId)
            .ToListAsync();

        var query = context.GalleryImages
            .AsNoTracking()
            .Where(image => image.IsActive);

        if (!string.IsNullOrWhiteSpace(cleanedCategory))
        {
            query = query.Where(image => image.Category == cleanedCategory);
        }

        var galleryImages = await query
            .OrderBy(image => image.DisplayOrder)
            .ThenBy(image => image.Id)
            .ToListAsync();

        var galleryImageDtos = mapper.Map<IReadOnlyList<GalleryImageDto>>(galleryImages);

        foreach (var galleryImageDto in galleryImageDtos)
        {
            galleryImageDto.IsFavorite = favoriteImageIds.Contains(galleryImageDto.Id);
        }

        return galleryImageDtos;
    }

    public async Task<IReadOnlyList<GalleryImageDto>> GetFavoriteGalleryImagesAsync(int userId)
    {
        var favoriteImages = await context.GalleryImageFavorites
            .AsNoTracking()
            .Where(favorite =>
                favorite.UserId == userId &&
                favorite.GalleryImage.IsActive)
            .OrderByDescending(favorite => favorite.CreatedAt)
            .Select(favorite => favorite.GalleryImage)
            .ToListAsync();

        var favoriteImageDtos = mapper.Map<IReadOnlyList<GalleryImageDto>>(favoriteImages);

        foreach (var favoriteImageDto in favoriteImageDtos)
        {
            favoriteImageDto.IsFavorite = true;
        }

        return favoriteImageDtos;
    }

    public async Task<GalleryImageDto> AddGalleryImageToFavoritesAsync(
        int galleryImageId,
        int userId)
    {
        var galleryImage = await context.GalleryImages
            .SingleOrDefaultAsync(image => image.Id == galleryImageId && image.IsActive);

        if (galleryImage == null)
        {
            throw new KeyNotFoundException("Gallery image not found.");
        }

        var favoriteAlreadyExists = await context.GalleryImageFavorites
            .AnyAsync(favorite =>
                favorite.UserId == userId &&
                favorite.GalleryImageId == galleryImageId);

        if (!favoriteAlreadyExists)
        {
            var favorite = new GalleryImageFavorite
            {
                UserId = userId,
                GalleryImageId = galleryImageId,
                CreatedAt = DateTime.UtcNow
            };

            context.GalleryImageFavorites.Add(favorite);

            await context.SaveChangesAsync();
        }

        var galleryImageDto = mapper.Map<GalleryImageDto>(galleryImage);
        galleryImageDto.IsFavorite = true;

        return galleryImageDto;
    }

    public async Task<GalleryImageDto> RemoveGalleryImageFromFavoritesAsync(
        int galleryImageId,
        int userId)
    {
        var galleryImage = await context.GalleryImages
            .SingleOrDefaultAsync(image => image.Id == galleryImageId && image.IsActive);

        if (galleryImage == null)
        {
            throw new KeyNotFoundException("Gallery image not found.");
        }

        var favorite = await context.GalleryImageFavorites
            .SingleOrDefaultAsync(favorite =>
                favorite.UserId == userId &&
                favorite.GalleryImageId == galleryImageId);

        if (favorite != null)
        {
            context.GalleryImageFavorites.Remove(favorite);

            await context.SaveChangesAsync();
        }

        var galleryImageDto = mapper.Map<GalleryImageDto>(galleryImage);
        galleryImageDto.IsFavorite = false;

        return galleryImageDto;
    }

    private static void ValidateCategory(string? category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            return;
        }

        if (!ValidCategories.Contains(category))
        {
            throw new ArgumentException("Invalid gallery category. Allowed categories are manicure, pedicure, makeup, and lashes.");
        }
    }
}