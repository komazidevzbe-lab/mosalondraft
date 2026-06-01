using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext : IdentityDbContext<
    AppUser,
    AppRole,
    int,
    IdentityUserClaim<int>,
    AppUserRole,
    IdentityUserLogin<int>,
    IdentityRoleClaim<int>,
    IdentityUserToken<int>>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<PasswordResetCode> PasswordResetCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ===============================
        // AppUser and AppUserRole relationship
        // This connects users to their assigned roles.
        // ===============================

        builder.Entity<AppUser>(entity =>
        {
            entity
                .HasMany(user => user.UserRoles)
                .WithOne(userRole => userRole.User)
                .HasForeignKey(userRole => userRole.UserId)
                .IsRequired();
        });

        // ===============================
        // AppRole and AppUserRole relationship
        // This connects roles to their assigned users.
        // ===============================

        builder.Entity<AppRole>(entity =>
        {
            entity
                .HasMany(role => role.UserRoles)
                .WithOne(userRole => userRole.Role)
                .HasForeignKey(userRole => userRole.RoleId)
                .IsRequired();
        });

        // ===============================
        // Password reset code relationship
        // Stores temporary reset codes for forgot-password flow.
        // ===============================

        builder.Entity<PasswordResetCode>(entity =>
        {
            entity.HasKey(resetCode => resetCode.Id);

            entity.Property(resetCode => resetCode.CodeHash)
                .IsRequired();

            entity.Property(resetCode => resetCode.IdentityResetToken)
                .IsRequired();

            entity.Property(resetCode => resetCode.CreatedAt)
                .IsRequired();

            entity.Property(resetCode => resetCode.ExpiresAt)
                .IsRequired();

            entity.HasOne(resetCode => resetCode.User)
                .WithMany()
                .HasForeignKey(resetCode => resetCode.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}