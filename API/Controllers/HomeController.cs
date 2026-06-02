using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class HomeController(IHomeService homeService) : BaseApiController
{
    // ===============================
    // Get Home page data
    // Returns all database-driven content needed by the Angular Home page.
    // ===============================

    [HttpGet]
    public async Task<ActionResult<HomePageDto>> GetHomePage()
    {
        var homePage = await homeService.GetHomePageAsync();

        return Ok(homePage);
    }
}