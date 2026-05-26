using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppUserRole : IdentityUserRole<int>
{
    // ===============================
    // User and role navigation properties
    // These allow Entity Framework to understand the user-role relationship.
    // ===============================

    public AppUser User { get; set; } = null!;

    public AppRole Role { get; set; } = null!;
}