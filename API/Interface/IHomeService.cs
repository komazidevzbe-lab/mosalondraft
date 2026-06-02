using API.DTOs;

namespace API.Interfaces;

public interface IHomeService
{
    Task<HomePageDto> GetHomePageAsync();
}