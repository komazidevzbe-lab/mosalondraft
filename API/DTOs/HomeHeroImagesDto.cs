namespace API.DTOs;

public class HomeHeroImagesDto
{
    public List<HomeHeroImageDto> Makeup { get; set; } = [];

    public List<HomeHeroImageDto> Lashes { get; set; } = [];

    public List<HomeHeroImageDto> Pedicure { get; set; } = [];

    public List<HomeHeroImageDto> Manicure { get; set; } = [];
}