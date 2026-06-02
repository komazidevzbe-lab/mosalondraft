using API.Entities;

namespace API.Data.SeedData;

public static class HomeSeedData
{
    private static readonly DateTime SeedCreatedAt = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static IEnumerable<HomePageContent> HomePageContents =>
    [
        new HomePageContent
        {
            Id = 1,
            SectionKey = "hero",
            EyebrowText = null,
            TitleLineOne = "Enhance Your Beauty",
            TitleLineOneHighlight = "Beauty",
            TitleLineTwo = "Elevate Your Confidence",
            TitleLineTwoHighlight = "Confidence",
            Description = "Expert nails and makeup services tailored for your unique glow. Look your best, feel your best.",
            ButtonText = "BOOK APPOINTMENT",
            ButtonLink = "/services",
            IsActive = true,
            CreatedAt = SeedCreatedAt,
            UpdatedAt = null
        },
        new HomePageContent
        {
            Id = 2,
            SectionKey = "about",
            EyebrowText = "ABOUT US",
            TitleLineOne = "Where Beauty Meets Confidence",
            TitleLineOneHighlight = "Beauty",
            TitleLineTwo = null,
            TitleLineTwoHighlight = null,
            Description = "At MO Beauty, we believe that self-care is more than a routine, it’s a form of self-love. Our expert team is dedicated to enhancing your natural beauty with precision, passion, and care.",
            ButtonText = null,
            ButtonLink = null,
            IsActive = true,
            CreatedAt = SeedCreatedAt,
            UpdatedAt = null
        }
    ];

    public static IEnumerable<HomeHeroImage> HomeHeroImages =>
        BuildHeroImages();

    public static IEnumerable<SalonService> SalonServices =>
    [
        new SalonService
        {
            Id = 1,
            Slug = "manicure",
            Name = "MANICURE",
            Description = "Polished to perfection. Every detail.",
            ImageUrl = "assets/home/nailcardcollage.svg",
            AltText = "Manicure nail design",
            IconUrl = "assets/home/nailpolishicon.svg",
            IconAltText = "Nail polish icon",
            DurationMinutes = 45,
            BasePrice = 250,
            IsFeaturedOnHome = true,
            IsActive = true,
            DisplayOrder = 1,
            CreatedAt = SeedCreatedAt,
            UpdatedAt = null
        },
        new SalonService
        {
            Id = 2,
            Slug = "makeup",
            Name = "MAKEUP",
            Description = "Flawless looks for every occasion.",
            ImageUrl = "assets/home/makeupcardcollage.svg",
            AltText = "Makeup service",
            IconUrl = "assets/home/makeupbrushicon.svg",
            IconAltText = "Makeup brush icon",
            DurationMinutes = 60,
            BasePrice = 550,
            IsFeaturedOnHome = true,
            IsActive = true,
            DisplayOrder = 2,
            CreatedAt = SeedCreatedAt,
            UpdatedAt = null
        },
        new SalonService
        {
            Id = 3,
            Slug = "pedicure",
            Name = "PEDICURE",
            Description = "Relaxing care for beautiful feet.",
            ImageUrl = "assets/home/pedicurecardcollage.svg",
            AltText = "Pedicure service",
            IconUrl = "assets/home/footprinticon.svg",
            IconAltText = "Bare foot print icon",
            DurationMinutes = 60,
            BasePrice = 300,
            IsFeaturedOnHome = true,
            IsActive = true,
            DisplayOrder = 3,
            CreatedAt = SeedCreatedAt,
            UpdatedAt = null
        },
        new SalonService
        {
            Id = 4,
            Slug = "lashes",
            Name = "BROWS & LASHES",
            Description = "Frame your beauty. Enhance your look.",
            ImageUrl = "assets/home/lashescardcollage.svg",
            AltText = "Brows and lashes service",
            IconUrl = "assets/home/eyelashesicon.svg",
            IconAltText = "Eyelashes icon",
            DurationMinutes = 45,
            BasePrice = 280,
            IsFeaturedOnHome = true,
            IsActive = true,
            DisplayOrder = 4,
            CreatedAt = SeedCreatedAt,
            UpdatedAt = null
        }
    ];

    public static IEnumerable<ClientReview> ClientReviews =>
    [
        new ClientReview
        {
            Id = 1,
            ClientName = "Lerato M.",
            Location = "Cape Town, GP",
            ReviewText = "Absolutely love my nails! The attention to detail and overall experience was beyond amazing.",
            Rating = 5,
            ImageUrl = "assets/gallery/makeup/3.svg",
            AltText = "Client review profile",
            IsApproved = true,
            IsFeatured = true,
            DisplayOrder = 1,
            CreatedAt = SeedCreatedAt,
            UpdatedAt = null
        },
        new ClientReview
        {
            Id = 2,
            ClientName = "Asanda R.",
            Location = "Joburg, SL",
            ReviewText = "The best pedicure I have ever had. So relaxing and my feet have never looked better!",
            Rating = 5,
            ImageUrl = "assets/gallery/makeup/8.svg",
            AltText = "Client review profile",
            IsApproved = true,
            IsFeatured = true,
            DisplayOrder = 2,
            CreatedAt = SeedCreatedAt,
            UpdatedAt = null
        },
        new ClientReview
        {
            Id = 3,
            ClientName = "Nontutuzelo L.",
            Location = "Bloemfontein, CBD",
            ReviewText = "My brows and lashes have never looked this good. I feel so confident every single day.",
            Rating = 5,
            ImageUrl = "assets/gallery/makeup/12.svg",
            AltText = "Client review profile",
            IsApproved = true,
            IsFeatured = true,
            DisplayOrder = 3,
            CreatedAt = SeedCreatedAt,
            UpdatedAt = null
        },
        new ClientReview
        {
            Id = 4,
            ClientName = "Thando K.",
            Location = "Kimberley, NC",
            ReviewText = "The makeup finish was soft, clean, and exactly what I wanted for my event.",
            Rating = 5,
            ImageUrl = "assets/gallery/makeup/15.svg",
            AltText = "Client review profile",
            IsApproved = true,
            IsFeatured = true,
            DisplayOrder = 4,
            CreatedAt = SeedCreatedAt,
            UpdatedAt = null
        },
        new ClientReview
        {
            Id = 5,
            ClientName = "Anele D.",
            Location = "Durban, KZN",
            ReviewText = "My manicure lasted beautifully and the whole appointment felt calm and professional.",
            Rating = 5,
            ImageUrl = "assets/gallery/makeup/18.svg",
            AltText = "Client review profile",
            IsApproved = true,
            IsFeatured = true,
            DisplayOrder = 5,
            CreatedAt = SeedCreatedAt,
            UpdatedAt = null
        },
        new ClientReview
        {
            Id = 6,
            ClientName = "Mpho S.",
            Location = "Pretoria, GP",
            ReviewText = "The lash set was neat, comfortable, and gave me the exact natural glam I asked for.",
            Rating = 5,
            ImageUrl = "assets/gallery/makeup/20.svg",
            AltText = "Client review profile",
            IsApproved = true,
            IsFeatured = true,
            DisplayOrder = 6,
            CreatedAt = SeedCreatedAt,
            UpdatedAt = null
        },
        new ClientReview
        {
            Id = 7,
            ClientName = "Zanele B.",
            Location = "Gqeberha, EC",
            ReviewText = "Beautiful service from start to finish. I left feeling pampered and confident.",
            Rating = 5,
            ImageUrl = "assets/gallery/makeup/5.svg",
            AltText = "Client review profile",
            IsApproved = true,
            IsFeatured = true,
            DisplayOrder = 7,
            CreatedAt = SeedCreatedAt,
            UpdatedAt = null
        },
        new ClientReview
        {
            Id = 8,
            ClientName = "Naledi P.",
            Location = "Polokwane, LP",
            ReviewText = "The team listened carefully and delivered a clean, elegant look that suited me perfectly.",
            Rating = 5,
            ImageUrl = "assets/gallery/makeup/9.svg",
            AltText = "Client review profile",
            IsApproved = true,
            IsFeatured = true,
            DisplayOrder = 8,
            CreatedAt = SeedCreatedAt,
            UpdatedAt = null
        },
        new ClientReview
        {
            Id = 9,
            ClientName = "Refilwe T.",
            Location = "Rustenburg, NW",
            ReviewText = "I loved the attention to detail. My nails looked polished, feminine, and flawless.",
            Rating = 5,
            ImageUrl = "assets/gallery/makeup/11.svg",
            AltText = "Client review profile",
            IsApproved = true,
            IsFeatured = true,
            DisplayOrder = 9,
            CreatedAt = SeedCreatedAt,
            UpdatedAt = null
        },
        new ClientReview
        {
            Id = 10,
            ClientName = "Boitumelo N.",
            Location = "Bloemfontein, FS",
            ReviewText = "The pedicure was relaxing and my feet looked fresh for weeks. I would definitely return.",
            Rating = 5,
            ImageUrl = "assets/gallery/makeup/14.svg",
            AltText = "Client review profile",
            IsApproved = true,
            IsFeatured = true,
            DisplayOrder = 10,
            CreatedAt = SeedCreatedAt,
            UpdatedAt = null
        }
    ];

    private static List<HomeHeroImage> BuildHeroImages()
    {
        var images = new List<HomeHeroImage>();
        var id = 1;

        AddImages(images, ref id, "makeup", "assets/gallery/makeup", 26, "Makeup beauty inspiration");
        AddImages(images, ref id, "lashes", "assets/gallery/lashes", 20, "Eyelash beauty inspiration");
        AddImages(images, ref id, "pedicure", "assets/gallery/pedicure", 24, "Pedicure beauty inspiration");
        AddImages(images, ref id, "manicure", "assets/gallery/manicure", 30, "Manicure nail design inspiration");

        return images;
    }

    private static void AddImages(
        List<HomeHeroImage> images,
        ref int id,
        string category,
        string folderPath,
        int imageCount,
        string altTextPrefix)
    {
        for (var imageNumber = 1; imageNumber <= imageCount; imageNumber++)
        {
            images.Add(new HomeHeroImage
            {
                Id = id,
                Category = category,
                ImageUrl = $"{folderPath}/{imageNumber}.svg",
                AltText = $"{altTextPrefix} {imageNumber}",
                DisplayOrder = imageNumber,
                IsActive = true,
                CreatedAt = SeedCreatedAt,
                UpdatedAt = null
            });

            id++;
        }
    }
}