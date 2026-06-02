using API.Data.SeedData;
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

    public DbSet<HomePageContent> HomePageContents { get; set; }

    public DbSet<HomeHeroImage> HomeHeroImages { get; set; }

    public DbSet<SalonService> SalonServices { get; set; }

    public DbSet<ClientReview> ClientReviews { get; set; }

    public DbSet<ContactMessage> ContactMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>(entity =>
        {
            entity
                .HasMany(user => user.UserRoles)
                .WithOne(userRole => userRole.User)
                .HasForeignKey(userRole => userRole.UserId)
                .IsRequired();
        });

        builder.Entity<AppRole>(entity =>
        {
            entity
                .HasMany(role => role.UserRoles)
                .WithOne(userRole => userRole.Role)
                .HasForeignKey(userRole => userRole.RoleId)
                .IsRequired();
        });

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

        builder.Entity<HomePageContent>(entity =>
        {
            entity.HasKey(content => content.Id);

            entity.Property(content => content.SectionKey)
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(content => content.SectionKey)
                .IsUnique();

            entity.Property(content => content.EyebrowText)
                .HasMaxLength(100);

            entity.Property(content => content.TitleLineOne)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(content => content.TitleLineOneHighlight)
                .HasMaxLength(100);

            entity.Property(content => content.TitleLineTwo)
                .HasMaxLength(200);

            entity.Property(content => content.TitleLineTwoHighlight)
                .HasMaxLength(100);

            entity.Property(content => content.Description)
                .HasMaxLength(1000)
                .IsRequired();

            entity.Property(content => content.ButtonText)
                .HasMaxLength(80);

            entity.Property(content => content.ButtonLink)
                .HasMaxLength(200);

            entity.Property(content => content.IsActive)
                .IsRequired();

            entity.Property(content => content.CreatedAt)
                .IsRequired();

            entity.HasData(HomeSeedData.HomePageContents);
        });

        builder.Entity<HomeHeroImage>(entity =>
        {
            entity.HasKey(image => image.Id);

            entity.Property(image => image.Category)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(image => image.ImageUrl)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(image => image.AltText)
                .HasMaxLength(250)
                .IsRequired();

            entity.Property(image => image.DisplayOrder)
                .IsRequired();

            entity.Property(image => image.IsActive)
                .IsRequired();

            entity.Property(image => image.CreatedAt)
                .IsRequired();

            entity.HasIndex(image => new
            {
                image.Category,
                image.DisplayOrder
            });

            entity.HasIndex(image => image.ImageUrl)
                .IsUnique();

            entity.HasData(HomeSeedData.HomeHeroImages);
        });

        builder.Entity<SalonService>(entity =>
        {
            entity.HasKey(service => service.Id);

            entity.Property(service => service.Slug)
                .HasMaxLength(80)
                .IsRequired();

            entity.HasIndex(service => service.Slug)
                .IsUnique();

            entity.Property(service => service.Name)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(service => service.Description)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(service => service.ImageUrl)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(service => service.AltText)
                .HasMaxLength(250)
                .IsRequired();

            entity.Property(service => service.IconUrl)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(service => service.IconAltText)
                .HasMaxLength(250)
                .IsRequired();

            entity.Property(service => service.DurationMinutes)
                .IsRequired();

            entity.Property(service => service.BasePrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(service => service.IsFeaturedOnHome)
                .IsRequired();

            entity.Property(service => service.IsActive)
                .IsRequired();

            entity.Property(service => service.DisplayOrder)
                .IsRequired();

            entity.Property(service => service.CreatedAt)
                .IsRequired();

            entity.HasIndex(service => service.DisplayOrder);

            entity.HasData(HomeSeedData.SalonServices);
        });

        builder.Entity<ClientReview>(entity =>
        {
            entity.HasKey(review => review.Id);

            entity.Property(review => review.ClientName)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(review => review.Location)
                .HasMaxLength(160)
                .IsRequired();

            entity.Property(review => review.ReviewText)
                .HasMaxLength(1000)
                .IsRequired();

            entity.Property(review => review.Rating)
                .IsRequired();

            entity.Property(review => review.ImageUrl)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(review => review.AltText)
                .HasMaxLength(250)
                .IsRequired();

            entity.Property(review => review.IsApproved)
                .IsRequired();

            entity.Property(review => review.IsFeatured)
                .IsRequired();

            entity.Property(review => review.DisplayOrder)
                .IsRequired();

            entity.Property(review => review.CreatedAt)
                .IsRequired();

            entity.HasOne(review => review.User)
                .WithMany()
                .HasForeignKey(review => review.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(review => review.UserId);

            entity.HasIndex(review => review.IsApproved);

            entity.HasIndex(review => new
            {
                review.IsFeatured,
                review.DisplayOrder
            });

            entity.HasData(HomeSeedData.ClientReviews);
        });

        builder.Entity<ContactMessage>(entity =>
        {
            entity.HasKey(message => message.Id);

            entity.Property(message => message.FullName)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(message => message.EmailAddress)
                .HasMaxLength(160)
                .IsRequired();

            entity.Property(message => message.PhoneNumber)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(message => message.Interest)
                .HasMaxLength(80)
                .IsRequired();

            entity.Property(message => message.Message)
                .HasMaxLength(1000)
                .IsRequired();

            entity.Property(message => message.MessageStatus)
                .HasMaxLength(40)
                .IsRequired();

            entity.Property(message => message.AdminResponse)
                .HasMaxLength(1000);

            entity.Property(message => message.SubmittedAt)
                .IsRequired();

            entity.Property(message => message.CreatedAt)
                .IsRequired();

            entity.HasOne(message => message.User)
                .WithMany()
                .HasForeignKey(message => message.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(message => message.UserId);

            entity.HasIndex(message => message.MessageStatus);

            entity.HasIndex(message => message.SubmittedAt);
        });
    }
}