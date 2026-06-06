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

    public DbSet<SalonServiceType> SalonServiceTypes { get; set; }

    public DbSet<SalonServiceLengthOption> SalonServiceLengthOptions { get; set; }

    public DbSet<ClientReview> ClientReviews { get; set; }

    public DbSet<ContactMessage> ContactMessages { get; set; }

    public DbSet<GalleryImage> GalleryImages { get; set; }

    public DbSet<GalleryImageFavorite> GalleryImageFavorites { get; set; }

    public DbSet<Booking> Bookings { get; set; }

    public DbSet<BookingServiceItem> BookingServiceItems { get; set; }

    public DbSet<BookingPayment> BookingPayments { get; set; }

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

            entity
                .HasMany(user => user.GalleryImageFavorites)
                .WithOne(favorite => favorite.User)
                .HasForeignKey(favorite => favorite.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasMany(user => user.Bookings)
                .WithOne(booking => booking.User)
                .HasForeignKey(booking => booking.UserId)
                .OnDelete(DeleteBehavior.Cascade);
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

        builder.Entity<SalonServiceType>(entity =>
        {
            entity.HasKey(serviceType => serviceType.Id);

            entity.Property(serviceType => serviceType.Name)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(serviceType => serviceType.DisplayOrder)
                .IsRequired();

            entity.Property(serviceType => serviceType.IsActive)
                .IsRequired();

            entity.Property(serviceType => serviceType.CreatedAt)
                .IsRequired();

            entity.HasOne(serviceType => serviceType.SalonService)
                .WithMany(service => service.ServiceTypes)
                .HasForeignKey(serviceType => serviceType.SalonServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(serviceType => new
            {
                serviceType.SalonServiceId,
                serviceType.Name
            })
            .IsUnique();

            entity.HasIndex(serviceType => new
            {
                serviceType.SalonServiceId,
                serviceType.DisplayOrder
            });

            entity.HasData(SalonBookingSeedData.SalonServiceTypes);
        });

        builder.Entity<SalonServiceLengthOption>(entity =>
        {
            entity.HasKey(lengthOption => lengthOption.Id);

            entity.Property(lengthOption => lengthOption.Name)
                .HasMaxLength(80)
                .IsRequired();

            entity.Property(lengthOption => lengthOption.PriceAddOn)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(lengthOption => lengthOption.DisplayOrder)
                .IsRequired();

            entity.Property(lengthOption => lengthOption.IsActive)
                .IsRequired();

            entity.Property(lengthOption => lengthOption.CreatedAt)
                .IsRequired();

            entity.HasOne(lengthOption => lengthOption.SalonService)
                .WithMany(service => service.LengthOptions)
                .HasForeignKey(lengthOption => lengthOption.SalonServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(lengthOption => new
            {
                lengthOption.SalonServiceId,
                lengthOption.Name
            })
            .IsUnique();

            entity.HasIndex(lengthOption => new
            {
                lengthOption.SalonServiceId,
                lengthOption.DisplayOrder
            });

            entity.HasData(SalonBookingSeedData.SalonServiceLengthOptions);
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

        builder.Entity<GalleryImage>(entity =>
        {
            entity.HasKey(image => image.Id);

            entity.Property(image => image.Title)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(image => image.Description)
                .HasMaxLength(160)
                .IsRequired();

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

            entity.HasIndex(image => image.ImageUrl)
                .IsUnique();

            entity.HasIndex(image => new
            {
                image.Category,
                image.DisplayOrder
            });

            entity.HasMany(image => image.Favorites)
                .WithOne(favorite => favorite.GalleryImage)
                .HasForeignKey(favorite => favorite.GalleryImageId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasData(GallerySeedData.GalleryImages);
        });

        builder.Entity<GalleryImageFavorite>(entity =>
        {
            entity.HasKey(favorite => favorite.Id);

            entity.Property(favorite => favorite.CreatedAt)
                .IsRequired();

            entity.HasIndex(favorite => new
            {
                favorite.UserId,
                favorite.GalleryImageId
            })
            .IsUnique();

            entity.HasIndex(favorite => favorite.UserId);

            entity.HasIndex(favorite => favorite.GalleryImageId);
        });

        builder.Entity<Booking>(entity =>
        {
            entity.HasKey(booking => booking.Id);

            entity.Property(booking => booking.BookingReference)
                .HasMaxLength(40)
                .IsRequired();

            entity.HasIndex(booking => booking.BookingReference)
                .IsUnique();

            entity.Property(booking => booking.BookingMode)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(booking => booking.ClientFullName)
                .HasMaxLength(160)
                .IsRequired();

            entity.Property(booking => booking.ClientEmailAddress)
                .HasMaxLength(160)
                .IsRequired();

            entity.Property(booking => booking.ClientPhoneNumber)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(booking => booking.PreferredContactMethod)
                .HasMaxLength(40)
                .IsRequired();

            entity.Property(booking => booking.TotalDurationMinutes)
                .IsRequired();

            entity.Property(booking => booking.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(booking => booking.DepositAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(booking => booking.BalanceAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(booking => booking.BookingStatus)
                .HasMaxLength(40)
                .IsRequired();

            entity.Property(booking => booking.PaymentStatus)
                .HasMaxLength(40)
                .IsRequired();

            entity.Property(booking => booking.CreatedAt)
                .IsRequired();

            entity.HasOne(booking => booking.User)
                .WithMany(user => user.Bookings)
                .HasForeignKey(booking => booking.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(booking => booking.UserId);

            entity.HasIndex(booking => booking.BookingStatus);

            entity.HasIndex(booking => booking.PaymentStatus);

            entity.HasIndex(booking => booking.CreatedAt);
        });

        builder.Entity<BookingServiceItem>(entity =>
        {
            entity.HasKey(item => item.Id);

            entity.Property(item => item.ServiceNameSnapshot)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(item => item.ServiceTypeNameSnapshot)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(item => item.LengthNameSnapshot)
                .HasMaxLength(80);

            entity.Property(item => item.BasePriceSnapshot)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(item => item.LengthAddOnPriceSnapshot)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(item => item.FinalPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(item => item.DurationMinutesSnapshot)
                .IsRequired();

            entity.Property(item => item.Notes)
                .HasMaxLength(1000);

            entity.Property(item => item.ReferenceImageType)
                .HasMaxLength(40);

            entity.Property(item => item.UploadedReferenceImageUrl)
                .HasMaxLength(500);

            entity.Property(item => item.ReferenceImagePreviewUrl)
                .HasMaxLength(500);

            entity.HasOne(item => item.Booking)
                .WithMany(booking => booking.Items)
                .HasForeignKey(item => item.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(item => item.SalonService)
                .WithMany(service => service.BookingItems)
                .HasForeignKey(item => item.SalonServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(item => item.SalonServiceType)
                .WithMany()
                .HasForeignKey(item => item.SalonServiceTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(item => item.SalonServiceLengthOption)
                .WithMany()
                .HasForeignKey(item => item.SalonServiceLengthOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(item => item.GalleryImage)
                .WithMany()
                .HasForeignKey(item => item.GalleryImageId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(item => item.BookingId);

            entity.HasIndex(item => item.SalonServiceId);

            entity.HasIndex(item => item.SalonServiceTypeId);

            entity.HasIndex(item => item.SalonServiceLengthOptionId);

            entity.HasIndex(item => item.GalleryImageId);

            entity.HasIndex(item => item.AppointmentDate);

            entity.HasIndex(item => new
            {
                item.AppointmentDate,
                item.StartTime,
                item.EndTime
            });
        });

        builder.Entity<BookingPayment>(entity =>
        {
            entity.HasKey(payment => payment.Id);

            entity.Property(payment => payment.PaymentProvider)
                .HasMaxLength(80)
                .IsRequired();

            entity.Property(payment => payment.PaymentStatus)
                .HasMaxLength(40)
                .IsRequired();

            entity.Property(payment => payment.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(payment => payment.MerchantReference)
                .HasMaxLength(120);

            entity.Property(payment => payment.GatewayPaymentId)
                .HasMaxLength(120);

            entity.Property(payment => payment.GatewayTransactionId)
                .HasMaxLength(120);

            entity.Property(payment => payment.RawGatewayResponse)
                .HasMaxLength(4000);

            entity.Property(payment => payment.CreatedAt)
                .IsRequired();

            entity.HasOne(payment => payment.Booking)
                .WithMany(booking => booking.Payments)
                .HasForeignKey(payment => payment.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(payment => payment.BookingId);

            entity.HasIndex(payment => payment.PaymentStatus);

            entity.HasIndex(payment => payment.MerchantReference);

            entity.HasIndex(payment => payment.CreatedAt);
        });
    }
}