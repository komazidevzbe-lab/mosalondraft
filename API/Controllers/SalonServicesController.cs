using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class SalonServicesController(
    ISalonServiceCatalogService salonServiceCatalogService
) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SalonServiceDto>>> GetActiveServices()
    {
        var services = await salonServiceCatalogService.GetActiveServicesAsync();

        return Ok(services);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SalonServiceDto>> GetServiceById(int id)
    {
        var service = await salonServiceCatalogService.GetActiveServiceByIdAsync(id);

        if (service == null)
        {
            return NotFound(new { message = "Service not found." });
        }

        return Ok(service);
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<SalonServiceDto>> GetServiceBySlug(string slug)
    {
        var service = await salonServiceCatalogService.GetActiveServiceBySlugAsync(slug);

        if (service == null)
        {
            return NotFound(new { message = "Service not found." });
        }

        return Ok(service);
    }
}