using API.Data;
using API.DTOs;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class SalonServiceCatalogService(DataContext context) : ISalonServiceCatalogService
{
    public async Task<IReadOnlyList<SalonServiceDto>> GetActiveServicesAsync()
    {
        return await context.SalonServices
            .AsNoTracking()
            .Include(service => service.ServiceTypes)
            .Include(service => service.LengthOptions)
            .Where(service => service.IsActive)
            .OrderBy(service => service.DisplayOrder)
            .Select(service => new SalonServiceDto
            {
                Id = service.Id,
                Slug = service.Slug,
                Title = service.Name,
                Description = service.Description,
                ImageUrl = service.ImageUrl,
                AltText = service.AltText,
                DurationMinutes = service.DurationMinutes,
                BasePrice = service.BasePrice,
                DisplayOrder = service.DisplayOrder,
                RequiresLength = service.LengthOptions.Any(length => length.IsActive),
                ServiceTypes = service.ServiceTypes
                    .Where(type => type.IsActive)
                    .OrderBy(type => type.DisplayOrder)
                    .Select(type => new SalonServiceTypeDto
                    {
                        Id = type.Id,
                        Name = type.Name,
                        DisplayOrder = type.DisplayOrder
                    })
                    .ToList(),
                LengthOptions = service.LengthOptions
                    .Where(length => length.IsActive)
                    .OrderBy(length => length.DisplayOrder)
                    .Select(length => new SalonServiceLengthOptionDto
                    {
                        Id = length.Id,
                        Name = length.Name,
                        PriceAddOn = length.PriceAddOn,
                        DisplayOrder = length.DisplayOrder
                    })
                    .ToList()
            })
            .ToListAsync();
    }

    public async Task<SalonServiceDto?> GetActiveServiceByIdAsync(int id)
    {
        var services = await GetActiveServicesAsync();
        return services.SingleOrDefault(service => service.Id == id);
    }

    public async Task<SalonServiceDto?> GetActiveServiceBySlugAsync(string slug)
    {
        var normalisedSlug = slug.Trim().ToLowerInvariant();

        var services = await GetActiveServicesAsync();

        return services.SingleOrDefault(service =>
            service.Slug.Equals(normalisedSlug, StringComparison.OrdinalIgnoreCase));
    }
}