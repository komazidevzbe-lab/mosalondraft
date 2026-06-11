using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        // ===============================
        // Database context
        // Registers SQL Server as the database provider.
        // ===============================

        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        });

        // ===============================
        // AutoMapper
        // Registers mapping profiles for DTO mapping.
        // ===============================

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // ===============================
        // HTTP context accessor
        // Allows services to read current request/user context when needed.
        // ===============================

        services.AddHttpContextAccessor();

        // ===============================
        // Core application services
        // These keep controllers thin and move business logic into services.
        // ===============================

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordResetService, PasswordResetService>();
        services.AddScoped<IHomeService, HomeService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IGalleryService, GalleryService>();

        // ===============================
        // Booking and service catalogue services
        // These support the full services booking flow.
        // ===============================

        services.AddScoped<ISalonServiceCatalogService, SalonServiceCatalogService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IPaymentService, PayFastPaymentService>();

        // ===============================
        // Cloudinary settings
        // Used for image/file upload storage.
        // ===============================

        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

        // ===============================
        // PayFast settings
        // Used for deposit payment initiation and verification.
        // ===============================

        services.Configure<PayFastSettings>(config.GetSection("PayFastSettings"));

        return services;
    }
}