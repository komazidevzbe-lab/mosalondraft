using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppRole : IdentityRole<int>
{
    // ===============================
    // Role relationship
    // This connects a role to all users assigned to that role.
    // ===============================

    public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
}