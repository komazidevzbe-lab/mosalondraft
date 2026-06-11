using API.Data;
using API.Extensions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ===============================
// Application service registration
// Keeps Program.cs clean and moves setup logic into extension files.
// ===============================

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

// ===============================
// Controllers
// Adds controller support and prevents object cycles in JSON responses.
// This follows the portal deployment pattern.
// ===============================

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// ===============================
// CORS
// Allows local Angular development and the deployed Azure App Service origin.
// This follows the portal deployment pattern where CORS is configured in Program.cs.
// ===============================

const string CorsPolicyName = "AppCors";

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200",
                "https://mosalon-app.azurewebsites.net"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// ===============================
// HTTP request pipeline
// Middleware order matters.
// This follows the portal deployment pattern:
// exception handling, HTTPS, CORS, auth, Angular static files, API controllers, fallback.
// ===============================

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors(CorsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

// ===============================
// Angular static files
// Serves the Angular production build from API/wwwroot.
// This works with Angular outputPath set to ../API/wwwroot.
// ===============================

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

// ===============================
// Angular route fallback
// Allows deployed Angular routes like /home, /services, /booking,
// /review-booking, /confirmation, /gallery and /contact to refresh
// without returning a 404.
// ===============================

app.MapFallbackToFile("index.html");

// ===============================
// Database migration
// Applies pending EF Core migrations when the API starts.
// MO Salon seed data is handled through EF/DataContext seed configuration,
// so no portal-specific runtime seed methods are copied here.
// ===============================

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();

    await context.Database.MigrateAsync();
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during database migration.");
}

app.Run();