using API.DTOs;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class GalleryController(
    IGalleryService galleryService
) : BaseApiController
{
    // ===============================
    // Get active gallery images
    // Returns all active gallery images.
    // Also marks which ones the logged-in user has saved as favorites.
    // Optional query filter:
    // /api/gallery?category=manicure
    // ===============================

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GalleryImageDto>>> GetGalleryImages(
        [FromQuery] string? category)
    {
        try
        {
            var userId = User.GetUserId();

            var galleryImages = await galleryService.GetActiveGalleryImagesAsync(
                category,
                userId);

            return Ok(galleryImages);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                category = ex.Message
            });
        }
    }

    // ===============================
    // Get active gallery images by category
    // Alternative route:
    // /api/gallery/category/manicure
    // ===============================

    [HttpGet("category/{category}")]
    public async Task<ActionResult<IReadOnlyList<GalleryImageDto>>> GetGalleryImagesByCategory(
        string category)
    {
        try
        {
            var userId = User.GetUserId();

            var galleryImages = await galleryService.GetActiveGalleryImagesAsync(
                category,
                userId);

            return Ok(galleryImages);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                category = ex.Message
            });
        }
    }

    // ===============================
    // Get logged-in user's favorite gallery images
    // /api/gallery/favorites
    // ===============================

    [HttpGet("favorites")]
    public async Task<ActionResult<IReadOnlyList<GalleryImageDto>>> GetFavoriteGalleryImages()
    {
        var userId = User.GetUserId();

        var favoriteImages = await galleryService.GetFavoriteGalleryImagesAsync(userId);

        return Ok(favoriteImages);
    }

    // ===============================
    // Add gallery image to favorites
    // /api/gallery/favorites/5
    // ===============================

    [HttpPost("favorites/{galleryImageId:int}")]
    public async Task<ActionResult<GalleryImageDto>> AddGalleryImageToFavorites(
        int galleryImageId)
    {
        try
        {
            var userId = User.GetUserId();

            var galleryImage = await galleryService.AddGalleryImageToFavoritesAsync(
                galleryImageId,
                userId);

            return Ok(galleryImage);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }

    // ===============================
    // Remove gallery image from favorites
    // /api/gallery/favorites/5
    // ===============================

    [HttpDelete("favorites/{galleryImageId:int}")]
    public async Task<ActionResult<GalleryImageDto>> RemoveGalleryImageFromFavorites(
        int galleryImageId)
    {
        try
        {
            var userId = User.GetUserId();

            var galleryImage = await galleryService.RemoveGalleryImageFromFavoritesAsync(
                galleryImageId,
                userId);

            return Ok(galleryImage);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }
}