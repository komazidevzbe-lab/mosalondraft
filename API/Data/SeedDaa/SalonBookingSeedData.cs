using API.Entities;

namespace API.Data.SeedData;

public static class SalonBookingSeedData
{
    public static readonly List<SalonServiceType> SalonServiceTypes =
    [
        // ===============================
        // Manicure service types
        // SalonServiceId 1 = manicure
        // ===============================

        new SalonServiceType
        {
            Id = 1,
            SalonServiceId = 1,
            Name = "Gel Manicure",
            DisplayOrder = 1,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new SalonServiceType
        {
            Id = 2,
            SalonServiceId = 1,
            Name = "Acrylic Set",
            DisplayOrder = 2,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new SalonServiceType
        {
            Id = 3,
            SalonServiceId = 1,
            Name = "Classic Manicure",
            DisplayOrder = 3,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new SalonServiceType
        {
            Id = 4,
            SalonServiceId = 1,
            Name = "Nail Art Add-on",
            DisplayOrder = 4,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },

        // ===============================
        // Makeup service types
        // SalonServiceId 2 = makeup
        // ===============================

        new SalonServiceType
        {
            Id = 5,
            SalonServiceId = 2,
            Name = "Soft Glam",
            DisplayOrder = 1,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new SalonServiceType
        {
            Id = 6,
            SalonServiceId = 2,
            Name = "Full Glam",
            DisplayOrder = 2,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new SalonServiceType
        {
            Id = 7,
            SalonServiceId = 2,
            Name = "Natural Makeup",
            DisplayOrder = 3,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new SalonServiceType
        {
            Id = 8,
            SalonServiceId = 2,
            Name = "Event Makeup",
            DisplayOrder = 4,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },

        // ===============================
        // Pedicure service types
        // SalonServiceId 3 = pedicure
        // ===============================

        new SalonServiceType
        {
            Id = 9,
            SalonServiceId = 3,
            Name = "Classic Pedicure",
            DisplayOrder = 1,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new SalonServiceType
        {
            Id = 10,
            SalonServiceId = 3,
            Name = "Spa Pedicure",
            DisplayOrder = 2,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new SalonServiceType
        {
            Id = 11,
            SalonServiceId = 3,
            Name = "Gel Pedicure",
            DisplayOrder = 3,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new SalonServiceType
        {
            Id = 12,
            SalonServiceId = 3,
            Name = "French Pedicure",
            DisplayOrder = 4,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },

        // ===============================
        // Lashes service types
        // SalonServiceId 4 = lashes
        // ===============================

        new SalonServiceType
        {
            Id = 13,
            SalonServiceId = 4,
            Name = "Classic Lash Extensions",
            DisplayOrder = 1,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new SalonServiceType
        {
            Id = 14,
            SalonServiceId = 4,
            Name = "Hybrid Lashes",
            DisplayOrder = 2,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new SalonServiceType
        {
            Id = 15,
            SalonServiceId = 4,
            Name = "Volume Lashes",
            DisplayOrder = 3,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        }
    ];

    public static readonly List<SalonServiceLengthOption> SalonServiceLengthOptions =
    [
        // ===============================
        // Manicure length options
        // SalonServiceId 1 = manicure
        // ===============================

        new SalonServiceLengthOption
        {
            Id = 1,
            SalonServiceId = 1,
            Name = "Short",
            PriceAddOn = 0,
            DisplayOrder = 1,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new SalonServiceLengthOption
        {
            Id = 2,
            SalonServiceId = 1,
            Name = "Medium",
            PriceAddOn = 60,
            DisplayOrder = 2,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new SalonServiceLengthOption
        {
            Id = 3,
            SalonServiceId = 1,
            Name = "Long",
            PriceAddOn = 120,
            DisplayOrder = 3,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new SalonServiceLengthOption
        {
            Id = 4,
            SalonServiceId = 1,
            Name = "Extra Long",
            PriceAddOn = 180,
            DisplayOrder = 4,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },

        // ===============================
        // Lashes length options
        // SalonServiceId 4 = lashes
        // ===============================

        new SalonServiceLengthOption
        {
            Id = 5,
            SalonServiceId = 4,
            Name = "Short",
            PriceAddOn = 0,
            DisplayOrder = 1,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new SalonServiceLengthOption
        {
            Id = 6,
            SalonServiceId = 4,
            Name = "Medium",
            PriceAddOn = 60,
            DisplayOrder = 2,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new SalonServiceLengthOption
        {
            Id = 7,
            SalonServiceId = 4,
            Name = "Long",
            PriceAddOn = 120,
            DisplayOrder = 3,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new SalonServiceLengthOption
        {
            Id = 8,
            SalonServiceId = 4,
            Name = "Extra Long",
            PriceAddOn = 180,
            DisplayOrder = 4,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        }
    ];
}