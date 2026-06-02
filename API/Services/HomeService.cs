using API.Data;
using API.DTOs;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class HomeService(DataContext context) : IHomeService
{
    public async Task<HomePageDto> GetHomePageAsync()
    {
        var heroContent = await GetPageContent("hero");
        var aboutContent = await GetPageContent("about");

        var heroImages = await context.HomeHeroImages
            .AsNoTracking()
            .Where(image => image.IsActive)
            .OrderBy(image => image.Category)
            .ThenBy(image => image.DisplayOrder)
            .Select(image => new HomeHeroImageDto
            {
                Id = image.Id,
                Category = image.Category,
                ImageUrl = image.ImageUrl,
                AltText = image.AltText,
                DisplayOrder = image.DisplayOrder
            })
            .ToListAsync();

        var featuredServices = await context.SalonServices
            .AsNoTracking()
            .Where(service => service.IsActive && service.IsFeaturedOnHome)
            .OrderBy(service => service.DisplayOrder)
            .Select(service => new HomeServiceDto
            {
                Id = service.Id,
                Slug = service.Slug,
                Name = service.Name,
                Description = service.Description,
                ImageUrl = service.ImageUrl,
                AltText = service.AltText,
                IconUrl = service.IconUrl,
                IconAltText = service.IconAltText,
                DurationMinutes = service.DurationMinutes,
                BasePrice = service.BasePrice,
                DisplayOrder = service.DisplayOrder
            })
            .ToListAsync();

        var featuredReviews = await context.ClientReviews
            .AsNoTracking()
            .Where(review => review.IsApproved && review.IsFeatured)
            .OrderBy(review => review.DisplayOrder)
            .Select(review => new HomeReviewDto
            {
                Id = review.Id,
                ClientName = review.ClientName,
                Location = review.Location,
                ReviewText = review.ReviewText,
                Rating = review.Rating,
                ImageUrl = review.ImageUrl,
                AltText = review.AltText,
                DisplayOrder = review.DisplayOrder,
                CreatedAt = review.CreatedAt
            })
            .ToListAsync();

        var approvedReviewCount = await context.ClientReviews
            .AsNoTracking()
            .CountAsync(review => review.IsApproved);

        var averageRating = approvedReviewCount == 0
            ? 0
            : await context.ClientReviews
                .AsNoTracking()
                .Where(review => review.IsApproved)
                .AverageAsync(review => review.Rating);

        return new HomePageDto
        {
            HeroContent = heroContent,
            AboutContent = aboutContent,
            HeroImages = new HomeHeroImagesDto
            {
                Makeup = heroImages.Where(image => image.Category == "makeup").ToList(),
                Lashes = heroImages.Where(image => image.Category == "lashes").ToList(),
                Pedicure = heroImages.Where(image => image.Category == "pedicure").ToList(),
                Manicure = heroImages.Where(image => image.Category == "manicure").ToList()
            },
            FeaturedServices = featuredServices,
            FeaturedReviews = featuredReviews,
            AverageRating = Math.Round(averageRating, 1),
            ReviewCount = approvedReviewCount
        };
    }

    private async Task<HomePageContentDto?> GetPageContent(string sectionKey)
    {
        return await context.HomePageContents
            .AsNoTracking()
            .Where(content => content.SectionKey == sectionKey && content.IsActive)
            .Select(content => new HomePageContentDto
            {
                SectionKey = content.SectionKey,
                EyebrowText = content.EyebrowText,
                TitleLineOne = content.TitleLineOne,
                TitleLineOneHighlight = content.TitleLineOneHighlight,
                TitleLineTwo = content.TitleLineTwo,
                TitleLineTwoHighlight = content.TitleLineTwoHighlight,
                Description = content.Description,
                ButtonText = content.ButtonText,
                ButtonLink = content.ButtonLink
            })
            .SingleOrDefaultAsync();
    }
}