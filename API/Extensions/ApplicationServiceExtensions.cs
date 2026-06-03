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
        // CORS policy
        // Allows the Angular client to communicate with the API during development.
        // ===============================

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy
                    .WithOrigins("http://localhost:4200", "https://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
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
        // Cloudinary settings
        // Used later for salon service, gallery, profile, or portfolio images.
        // ===============================

        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

        return services;
    }
}