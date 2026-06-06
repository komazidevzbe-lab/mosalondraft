using API.DTOs;

namespace API.Interfaces;

public interface ISalonServiceCatalogService
{
    Task<IReadOnlyList<SalonServiceDto>> GetActiveServicesAsync();

    Task<SalonServiceDto?> GetActiveServiceByIdAsync(int id);

    Task<SalonServiceDto?> GetActiveServiceBySlugAsync(string slug);
}