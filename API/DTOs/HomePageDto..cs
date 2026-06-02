namespace API.DTOs;

public class HomePageDto
{
    public HomePageContentDto? HeroContent { get; set; }

    public HomePageContentDto? AboutContent { get; set; }

    public HomeHeroImagesDto HeroImages { get; set; } = new();

    public List<HomeServiceDto> FeaturedServices { get; set; } = [];

    public List<HomeReviewDto> FeaturedReviews { get; set; } = [];

    public double AverageRating { get; set; }

    public int ReviewCount { get; set; }
}