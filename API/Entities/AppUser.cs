using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppUser : IdentityUser<int>
{
    // ===============================
    // User profile details
    // These fields come from the signup form.
    // ===============================

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public DateOnly? JoinDate { get; set; }

    // ===============================
    // Identity role relationship
    // This connects users to their assigned roles.
    // ===============================

    public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();

    // ===============================
    // Gallery favorites relationship
    // This stores the gallery images saved by the logged-in user.
    // ===============================

    public ICollection<GalleryImageFavorite> GalleryImageFavorites { get; set; } = new List<GalleryImageFavorite>();

    // ===============================
    // Booking relationship
    // This stores bookings created by the logged-in user.
    // ===============================

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}