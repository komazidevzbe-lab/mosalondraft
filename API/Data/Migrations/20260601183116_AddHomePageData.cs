using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHomePageData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AltText = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientReviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeHeroImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AltText = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeHeroImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomePageContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EyebrowText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TitleLineOne = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TitleLineOneHighlight = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TitleLineTwo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TitleLineTwoHighlight = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ButtonText = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    ButtonLink = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePageContents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalonServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Slug = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AltText = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IconUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IconAltText = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsFeaturedOnHome = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalonServices", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ClientReviews",
                columns: new[] { "Id", "AltText", "ClientName", "CreatedAt", "DisplayOrder", "ImageUrl", "IsApproved", "IsFeatured", "Location", "Rating", "ReviewText", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Client review profile", "Lerato M.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "assets/gallery/makeup/3.svg", true, true, "Cape Town, GP", 5, "Absolutely love my nails! The attention to detail and overall experience was beyond amazing.", null },
                    { 2, "Client review profile", "Asanda R.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "assets/gallery/makeup/8.svg", true, true, "Joburg, SL", 5, "The best pedicure I have ever had. So relaxing and my feet have never looked better!", null },
                    { 3, "Client review profile", "Nontutuzelo L.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "assets/gallery/makeup/12.svg", true, true, "Bloemfontein, CBD", 5, "My brows and lashes have never looked this good. I feel so confident every single day.", null }
                });

            migrationBuilder.InsertData(
                table: "HomeHeroImages",
                columns: new[] { "Id", "AltText", "Category", "CreatedAt", "DisplayOrder", "ImageUrl", "IsActive", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Makeup beauty inspiration 1", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "assets/gallery/makeup/1.svg", true, null },
                    { 2, "Makeup beauty inspiration 2", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "assets/gallery/makeup/2.svg", true, null },
                    { 3, "Makeup beauty inspiration 3", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "assets/gallery/makeup/3.svg", true, null },
                    { 4, "Makeup beauty inspiration 4", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "assets/gallery/makeup/4.svg", true, null },
                    { 5, "Makeup beauty inspiration 5", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, "assets/gallery/makeup/5.svg", true, null },
                    { 6, "Makeup beauty inspiration 6", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, "assets/gallery/makeup/6.svg", true, null },
                    { 7, "Makeup beauty inspiration 7", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, "assets/gallery/makeup/7.svg", true, null },
                    { 8, "Makeup beauty inspiration 8", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8, "assets/gallery/makeup/8.svg", true, null },
                    { 9, "Makeup beauty inspiration 9", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, "assets/gallery/makeup/9.svg", true, null },
                    { 10, "Makeup beauty inspiration 10", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10, "assets/gallery/makeup/10.svg", true, null },
                    { 11, "Makeup beauty inspiration 11", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, "assets/gallery/makeup/11.svg", true, null },
                    { 12, "Makeup beauty inspiration 12", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 12, "assets/gallery/makeup/12.svg", true, null },
                    { 13, "Makeup beauty inspiration 13", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 13, "assets/gallery/makeup/13.svg", true, null },
                    { 14, "Makeup beauty inspiration 14", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 14, "assets/gallery/makeup/14.svg", true, null },
                    { 15, "Makeup beauty inspiration 15", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 15, "assets/gallery/makeup/15.svg", true, null },
                    { 16, "Makeup beauty inspiration 16", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 16, "assets/gallery/makeup/16.svg", true, null },
                    { 17, "Makeup beauty inspiration 17", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 17, "assets/gallery/makeup/17.svg", true, null },
                    { 18, "Makeup beauty inspiration 18", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 18, "assets/gallery/makeup/18.svg", true, null },
                    { 19, "Makeup beauty inspiration 19", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 19, "assets/gallery/makeup/19.svg", true, null },
                    { 20, "Makeup beauty inspiration 20", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 20, "assets/gallery/makeup/20.svg", true, null },
                    { 21, "Makeup beauty inspiration 21", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 21, "assets/gallery/makeup/21.svg", true, null },
                    { 22, "Makeup beauty inspiration 22", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 22, "assets/gallery/makeup/22.svg", true, null },
                    { 23, "Makeup beauty inspiration 23", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 23, "assets/gallery/makeup/23.svg", true, null },
                    { 24, "Makeup beauty inspiration 24", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24, "assets/gallery/makeup/24.svg", true, null },
                    { 25, "Makeup beauty inspiration 25", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 25, "assets/gallery/makeup/25.svg", true, null },
                    { 26, "Makeup beauty inspiration 26", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 26, "assets/gallery/makeup/26.svg", true, null },
                    { 27, "Eyelash beauty inspiration 1", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "assets/gallery/lashes/1.svg", true, null },
                    { 28, "Eyelash beauty inspiration 2", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "assets/gallery/lashes/2.svg", true, null },
                    { 29, "Eyelash beauty inspiration 3", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "assets/gallery/lashes/3.svg", true, null },
                    { 30, "Eyelash beauty inspiration 4", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "assets/gallery/lashes/4.svg", true, null },
                    { 31, "Eyelash beauty inspiration 5", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, "assets/gallery/lashes/5.svg", true, null },
                    { 32, "Eyelash beauty inspiration 6", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, "assets/gallery/lashes/6.svg", true, null },
                    { 33, "Eyelash beauty inspiration 7", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, "assets/gallery/lashes/7.svg", true, null },
                    { 34, "Eyelash beauty inspiration 8", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8, "assets/gallery/lashes/8.svg", true, null },
                    { 35, "Eyelash beauty inspiration 9", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, "assets/gallery/lashes/9.svg", true, null },
                    { 36, "Eyelash beauty inspiration 10", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10, "assets/gallery/lashes/10.svg", true, null },
                    { 37, "Eyelash beauty inspiration 11", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, "assets/gallery/lashes/11.svg", true, null },
                    { 38, "Eyelash beauty inspiration 12", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 12, "assets/gallery/lashes/12.svg", true, null },
                    { 39, "Eyelash beauty inspiration 13", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 13, "assets/gallery/lashes/13.svg", true, null },
                    { 40, "Eyelash beauty inspiration 14", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 14, "assets/gallery/lashes/14.svg", true, null },
                    { 41, "Eyelash beauty inspiration 15", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 15, "assets/gallery/lashes/15.svg", true, null },
                    { 42, "Eyelash beauty inspiration 16", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 16, "assets/gallery/lashes/16.svg", true, null },
                    { 43, "Eyelash beauty inspiration 17", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 17, "assets/gallery/lashes/17.svg", true, null },
                    { 44, "Eyelash beauty inspiration 18", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 18, "assets/gallery/lashes/18.svg", true, null },
                    { 45, "Eyelash beauty inspiration 19", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 19, "assets/gallery/lashes/19.svg", true, null },
                    { 46, "Eyelash beauty inspiration 20", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 20, "assets/gallery/lashes/20.svg", true, null },
                    { 47, "Pedicure beauty inspiration 1", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "assets/gallery/pedicure/1.svg", true, null },
                    { 48, "Pedicure beauty inspiration 2", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "assets/gallery/pedicure/2.svg", true, null },
                    { 49, "Pedicure beauty inspiration 3", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "assets/gallery/pedicure/3.svg", true, null },
                    { 50, "Pedicure beauty inspiration 4", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "assets/gallery/pedicure/4.svg", true, null },
                    { 51, "Pedicure beauty inspiration 5", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, "assets/gallery/pedicure/5.svg", true, null },
                    { 52, "Pedicure beauty inspiration 6", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, "assets/gallery/pedicure/6.svg", true, null },
                    { 53, "Pedicure beauty inspiration 7", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, "assets/gallery/pedicure/7.svg", true, null },
                    { 54, "Pedicure beauty inspiration 8", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8, "assets/gallery/pedicure/8.svg", true, null },
                    { 55, "Pedicure beauty inspiration 9", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, "assets/gallery/pedicure/9.svg", true, null },
                    { 56, "Pedicure beauty inspiration 10", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10, "assets/gallery/pedicure/10.svg", true, null },
                    { 57, "Pedicure beauty inspiration 11", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, "assets/gallery/pedicure/11.svg", true, null },
                    { 58, "Pedicure beauty inspiration 12", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 12, "assets/gallery/pedicure/12.svg", true, null },
                    { 59, "Pedicure beauty inspiration 13", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 13, "assets/gallery/pedicure/13.svg", true, null },
                    { 60, "Pedicure beauty inspiration 14", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 14, "assets/gallery/pedicure/14.svg", true, null },
                    { 61, "Pedicure beauty inspiration 15", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 15, "assets/gallery/pedicure/15.svg", true, null },
                    { 62, "Pedicure beauty inspiration 16", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 16, "assets/gallery/pedicure/16.svg", true, null },
                    { 63, "Pedicure beauty inspiration 17", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 17, "assets/gallery/pedicure/17.svg", true, null },
                    { 64, "Pedicure beauty inspiration 18", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 18, "assets/gallery/pedicure/18.svg", true, null },
                    { 65, "Pedicure beauty inspiration 19", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 19, "assets/gallery/pedicure/19.svg", true, null },
                    { 66, "Pedicure beauty inspiration 20", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 20, "assets/gallery/pedicure/20.svg", true, null },
                    { 67, "Pedicure beauty inspiration 21", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 21, "assets/gallery/pedicure/21.svg", true, null },
                    { 68, "Pedicure beauty inspiration 22", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 22, "assets/gallery/pedicure/22.svg", true, null },
                    { 69, "Pedicure beauty inspiration 23", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 23, "assets/gallery/pedicure/23.svg", true, null },
                    { 70, "Pedicure beauty inspiration 24", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24, "assets/gallery/pedicure/24.svg", true, null },
                    { 71, "Manicure nail design inspiration 1", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "assets/gallery/manicure/1.svg", true, null },
                    { 72, "Manicure nail design inspiration 2", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "assets/gallery/manicure/2.svg", true, null },
                    { 73, "Manicure nail design inspiration 3", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "assets/gallery/manicure/3.svg", true, null },
                    { 74, "Manicure nail design inspiration 4", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "assets/gallery/manicure/4.svg", true, null },
                    { 75, "Manicure nail design inspiration 5", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, "assets/gallery/manicure/5.svg", true, null },
                    { 76, "Manicure nail design inspiration 6", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, "assets/gallery/manicure/6.svg", true, null },
                    { 77, "Manicure nail design inspiration 7", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, "assets/gallery/manicure/7.svg", true, null },
                    { 78, "Manicure nail design inspiration 8", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8, "assets/gallery/manicure/8.svg", true, null },
                    { 79, "Manicure nail design inspiration 9", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, "assets/gallery/manicure/9.svg", true, null },
                    { 80, "Manicure nail design inspiration 10", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10, "assets/gallery/manicure/10.svg", true, null },
                    { 81, "Manicure nail design inspiration 11", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, "assets/gallery/manicure/11.svg", true, null },
                    { 82, "Manicure nail design inspiration 12", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 12, "assets/gallery/manicure/12.svg", true, null },
                    { 83, "Manicure nail design inspiration 13", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 13, "assets/gallery/manicure/13.svg", true, null },
                    { 84, "Manicure nail design inspiration 14", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 14, "assets/gallery/manicure/14.svg", true, null },
                    { 85, "Manicure nail design inspiration 15", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 15, "assets/gallery/manicure/15.svg", true, null },
                    { 86, "Manicure nail design inspiration 16", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 16, "assets/gallery/manicure/16.svg", true, null },
                    { 87, "Manicure nail design inspiration 17", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 17, "assets/gallery/manicure/17.svg", true, null },
                    { 88, "Manicure nail design inspiration 18", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 18, "assets/gallery/manicure/18.svg", true, null },
                    { 89, "Manicure nail design inspiration 19", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 19, "assets/gallery/manicure/19.svg", true, null },
                    { 90, "Manicure nail design inspiration 20", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 20, "assets/gallery/manicure/20.svg", true, null },
                    { 91, "Manicure nail design inspiration 21", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 21, "assets/gallery/manicure/21.svg", true, null },
                    { 92, "Manicure nail design inspiration 22", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 22, "assets/gallery/manicure/22.svg", true, null },
                    { 93, "Manicure nail design inspiration 23", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 23, "assets/gallery/manicure/23.svg", true, null },
                    { 94, "Manicure nail design inspiration 24", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24, "assets/gallery/manicure/24.svg", true, null },
                    { 95, "Manicure nail design inspiration 25", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 25, "assets/gallery/manicure/25.svg", true, null },
                    { 96, "Manicure nail design inspiration 26", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 26, "assets/gallery/manicure/26.svg", true, null },
                    { 97, "Manicure nail design inspiration 27", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 27, "assets/gallery/manicure/27.svg", true, null },
                    { 98, "Manicure nail design inspiration 28", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 28, "assets/gallery/manicure/28.svg", true, null },
                    { 99, "Manicure nail design inspiration 29", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 29, "assets/gallery/manicure/29.svg", true, null },
                    { 100, "Manicure nail design inspiration 30", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 30, "assets/gallery/manicure/30.svg", true, null }
                });

            migrationBuilder.InsertData(
                table: "HomePageContents",
                columns: new[] { "Id", "ButtonLink", "ButtonText", "CreatedAt", "Description", "EyebrowText", "IsActive", "SectionKey", "TitleLineOne", "TitleLineOneHighlight", "TitleLineTwo", "TitleLineTwoHighlight", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "/services", "BOOK APPOINTMENT", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Expert nails and makeup services tailored for your unique glow. Look your best, feel your best.", null, true, "hero", "Enhance Your Beauty", "Beauty", "Elevate Your Confidence", "Confidence", null },
                    { 2, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "At MO Beauty, we believe that self-care is more than a routine, it’s a form of self-love. Our expert team is dedicated to enhancing your natural beauty with precision, passion, and care.", "ABOUT US", true, "about", "Where Beauty Meets Confidence", "Beauty", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "SalonServices",
                columns: new[] { "Id", "AltText", "BasePrice", "CreatedAt", "Description", "DisplayOrder", "DurationMinutes", "IconAltText", "IconUrl", "ImageUrl", "IsActive", "IsFeaturedOnHome", "Name", "Slug", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Manicure nail design", 250m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Polished to perfection. Every detail.", 1, 45, "Nail polish icon", "assets/home/nailpolishicon.svg", "assets/home/nailcardcollage.svg", true, true, "MANICURE", "manicure", null },
                    { 2, "Makeup service", 550m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Flawless looks for every occasion.", 2, 60, "Makeup brush icon", "assets/home/makeupbrushicon.svg", "assets/home/makeupcardcollage.svg", true, true, "MAKEUP", "makeup", null },
                    { 3, "Pedicure service", 300m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Relaxing care for beautiful feet.", 3, 60, "Bare foot print icon", "assets/home/footprinticon.svg", "assets/home/pedicurecardcollage.svg", true, true, "PEDICURE", "pedicure", null },
                    { 4, "Brows and lashes service", 280m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Frame your beauty. Enhance your look.", 4, 45, "Eyelashes icon", "assets/home/eyelashesicon.svg", "assets/home/lashescardcollage.svg", true, true, "BROWS & LASHES", "lashes", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientReviews_IsApproved",
                table: "ClientReviews",
                column: "IsApproved");

            migrationBuilder.CreateIndex(
                name: "IX_ClientReviews_IsFeatured_DisplayOrder",
                table: "ClientReviews",
                columns: new[] { "IsFeatured", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeHeroImages_Category_DisplayOrder",
                table: "HomeHeroImages",
                columns: new[] { "Category", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeHeroImages_ImageUrl",
                table: "HomeHeroImages",
                column: "ImageUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomePageContents_SectionKey",
                table: "HomePageContents",
                column: "SectionKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalonServices_DisplayOrder",
                table: "SalonServices",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_SalonServices_Slug",
                table: "SalonServices",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientReviews");

            migrationBuilder.DropTable(
                name: "HomeHeroImages");

            migrationBuilder.DropTable(
                name: "HomePageContents");

            migrationBuilder.DropTable(
                name: "SalonServices");
        }
    }
}
