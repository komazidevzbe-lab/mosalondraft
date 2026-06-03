using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGalleryImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GalleryImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
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
                    table.PrimaryKey("PK_GalleryImages", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "GalleryImages",
                columns: new[] { "Id", "AltText", "Category", "CreatedAt", "Description", "DisplayOrder", "ImageUrl", "IsActive", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Manicure nail collage inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 1, "assets/home/nailcardcollage.svg", true, "Blush Bloom", null },
                    { 2, "Makeup collage inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 2, "assets/home/makeupcardcollage.svg", true, "Soft Glam Glow", null },
                    { 3, "Pedicure collage inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 3, "assets/home/pedicurecardcollage.svg", true, "Pearl Petal Steps", null },
                    { 4, "Brows and lashes collage inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 4, "assets/home/lashescardcollage.svg", true, "Velvet Wink", null },
                    { 5, "Crystal rose manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 5, "assets/gallery/manicure/17.svg", true, "Crystal Rose Spell", null },
                    { 6, "Golden sand pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 6, "assets/gallery/pedicure/20.svg", true, "Golden Sand Glow", null },
                    { 7, "Moonlit glamour makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 7, "assets/gallery/makeup/12.svg", true, "Moonlit Glamour", null },
                    { 8, "Fairy lash inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 8, "assets/gallery/lashes/4.svg", true, "Fairy Lash Flutter", null },
                    { 9, "Sugar pearl pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 9, "assets/gallery/pedicure/3.svg", true, "Sugar Pearl Toes", null },
                    { 10, "Pink diamond manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 10, "assets/gallery/manicure/8.svg", true, "Pink Diamond Bloom", null },
                    { 11, "Midnight lash inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 11, "assets/gallery/lashes/19.svg", true, "Midnight Lash Charm", null },
                    { 12, "Champagne halo makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 12, "assets/gallery/makeup/1.svg", true, "Champagne Halo", null },
                    { 13, "Enchanted petal manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 13, "assets/gallery/manicure/2.svg", true, "Enchanted Petals", null },
                    { 14, "Blush sole pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 14, "assets/gallery/pedicure/14.svg", true, "Blush Sole Dream", null },
                    { 15, "Royal glow makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 15, "assets/gallery/makeup/18.svg", true, "Royal Glow Beat", null },
                    { 16, "Soft siren lashes inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 16, "assets/gallery/lashes/7.svg", true, "Soft Siren Flutter", null },
                    { 17, "Petal silk pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 17, "assets/gallery/pedicure/9.svg", true, "Petal Silk Steps", null },
                    { 18, "Rose quartz manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 18, "assets/gallery/manicure/20.svg", true, "Rose Quartz Tips", null },
                    { 19, "Starlight makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 19, "assets/gallery/makeup/6.svg", true, "Starlight Face Beat", null },
                    { 20, "Goddess lash inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 20, "assets/gallery/lashes/15.svg", true, "Goddess Lash Veil", null },
                    { 21, "Glitter garden manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 21, "assets/gallery/manicure/11.svg", true, "Glitter Garden", null },
                    { 22, "Fresh pearl pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 22, "assets/gallery/pedicure/1.svg", true, "Fresh Pearl Walk", null },
                    { 23, "Whisper wing lashes inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 23, "assets/gallery/lashes/10.svg", true, "Whisper Wing Lashes", null },
                    { 24, "Golden hour makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 24, "assets/gallery/makeup/15.svg", true, "Golden Hour Muse", null },
                    { 25, "Sugar blush pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 25, "assets/gallery/pedicure/18.svg", true, "Sugar Blush Pedi", null },
                    { 26, "Velvet rose manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 26, "assets/gallery/manicure/5.svg", true, "Velvet Rose Nails", null },
                    { 27, "Diamond dust makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 27, "assets/gallery/makeup/9.svg", true, "Diamond Dust Glow", null },
                    { 28, "Angel wing lashes inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 28, "assets/gallery/lashes/2.svg", true, "Angel Wing Flick", null },
                    { 29, "Pink potion manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 29, "assets/gallery/manicure/14.svg", true, "Pink Potion Set", null },
                    { 30, "Cream cloud pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 30, "assets/gallery/pedicure/6.svg", true, "Cream Cloud Toes", null },
                    { 31, "Drama queen lashes inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 31, "assets/gallery/lashes/18.svg", true, "Drama Queen Lashes", null },
                    { 32, "Bronze belle makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 32, "assets/gallery/makeup/3.svg", true, "Bronze Belle Glow", null },
                    { 33, "Silk slipper pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 33, "assets/gallery/pedicure/11.svg", true, "Silk Slipper Shine", null },
                    { 34, "Fairytale french manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 34, "assets/gallery/manicure/1.svg", true, "Fairytale French", null },
                    { 35, "Glamour crown makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 35, "assets/gallery/makeup/20.svg", true, "Glamour Crown", null },
                    { 36, "Butterfly wink lashes inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 36, "assets/gallery/lashes/12.svg", true, "Butterfly Wink", null },
                    { 37, "Ruby rose manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 37, "assets/gallery/manicure/18.svg", true, "Ruby Rose Tips", null },
                    { 38, "Crystal pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 38, "assets/gallery/pedicure/4.svg", true, "Crystal Pedi Glow", null },
                    { 39, "Velvet smoke makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 39, "assets/gallery/makeup/11.svg", true, "Velvet Smoke", null },
                    { 40, "Luxe lash inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 40, "assets/gallery/lashes/6.svg", true, "Luxe Lash Spell", null },
                    { 41, "Rosewater pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 41, "assets/gallery/pedicure/16.svg", true, "Rosewater Steps", null },
                    { 42, "Golden petal manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 42, "assets/gallery/manicure/9.svg", true, "Golden Petal Nails", null },
                    { 43, "Doll eye lashes inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 43, "assets/gallery/lashes/20.svg", true, "Doll Eye Dream", null },
                    { 44, "Soft royal makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 44, "assets/gallery/makeup/5.svg", true, "Soft Royal Flush", null },
                    { 45, "Blush crystal manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 45, "assets/gallery/manicure/13.svg", true, "Blush Crystal Set", null },
                    { 46, "Peach pearl pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 46, "assets/gallery/pedicure/8.svg", true, "Peach Pearl Pedi", null },
                    { 47, "Muse magic makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 47, "assets/gallery/makeup/14.svg", true, "Muse Magic", null },
                    { 48, "Feather kiss lashes inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 48, "assets/gallery/lashes/1.svg", true, "Feather Kiss Lashes", null },
                    { 49, "Opal toe pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 49, "assets/gallery/pedicure/19.svg", true, "Opal Toe Shine", null },
                    { 50, "Royal rose manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 50, "assets/gallery/manicure/4.svg", true, "Royal Rose Set", null },
                    { 51, "Starlit lash inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 51, "assets/gallery/lashes/14.svg", true, "Starlit Lash Line", null },
                    { 52, "Honey glow makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 52, "assets/gallery/makeup/8.svg", true, "Honey Glow Glam", null },
                    { 53, "Diamond petal manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 53, "assets/gallery/manicure/16.svg", true, "Diamond Petal Tips", null },
                    { 54, "Barefoot blush pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 54, "assets/gallery/pedicure/2.svg", true, "Barefoot Blush", null },
                    { 55, "Pearl skin makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 55, "assets/gallery/makeup/17.svg", true, "Pearl Skin Glow", null },
                    { 56, "Soft volume lashes inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 56, "assets/gallery/lashes/9.svg", true, "Soft Volume Spell", null },
                    { 57, "Rose gold pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 57, "assets/gallery/pedicure/13.svg", true, "Rose Gold Steps", null },
                    { 58, "Candy floss manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 58, "assets/gallery/manicure/7.svg", true, "Candy Floss Tips", null },
                    { 59, "Velvet flutter lashes inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 59, "assets/gallery/lashes/17.svg", true, "Velvet Flutter", null },
                    { 60, "Pink moon makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 60, "assets/gallery/makeup/2.svg", true, "Pink Moon Glam", null },
                    { 61, "Lavish bloom manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 61, "assets/gallery/manicure/19.svg", true, "Lavish Bloom Nails", null },
                    { 62, "Glossy garden pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 62, "assets/gallery/pedicure/7.svg", true, "Glossy Garden Pedi", null },
                    { 63, "Satin glow makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 63, "assets/gallery/makeup/10.svg", true, "Satin Glow Beat", null },
                    { 64, "Cat eye lashes inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 64, "assets/gallery/lashes/5.svg", true, "Cat Eye Flutter", null },
                    { 65, "Champagne pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 65, "assets/gallery/pedicure/17.svg", true, "Champagne Pedi Glow", null },
                    { 66, "Dreamy pink manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 66, "assets/gallery/manicure/3.svg", true, "Dreamy Pink Set", null },
                    { 67, "Royal wing lashes inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 67, "assets/gallery/lashes/13.svg", true, "Royal Wing Lashes", null },
                    { 68, "Glazed goddess makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 68, "assets/gallery/makeup/19.svg", true, "Glazed Goddess", null },
                    { 69, "Sparkle rose manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 69, "assets/gallery/manicure/15.svg", true, "Sparkle Rose Tips", null },
                    { 70, "Soft petal pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 70, "assets/gallery/pedicure/5.svg", true, "Soft Petal Pedi", null },
                    { 71, "Celestial makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 71, "assets/gallery/makeup/4.svg", true, "Celestial Soft Glam", null },
                    { 72, "Glam doll lashes inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 72, "assets/gallery/lashes/16.svg", true, "Glam Doll Lashes", null },
                    { 73, "Silky blush pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 73, "assets/gallery/pedicure/10.svg", true, "Silky Blush Toes", null },
                    { 74, "Magic rose manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 74, "assets/gallery/manicure/12.svg", true, "Magic Rose Nails", null },
                    { 75, "Flutter fantasy lashes inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 75, "assets/gallery/lashes/8.svg", true, "Flutter Fantasy", null },
                    { 76, "Soft fire makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 76, "assets/gallery/makeup/16.svg", true, "Soft Fire Glam", null },
                    { 77, "Pink velvet manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 77, "assets/gallery/manicure/6.svg", true, "Pink Velvet Tips", null },
                    { 78, "Pearl glow pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 78, "assets/gallery/pedicure/15.svg", true, "Pearl Glow Pedi", null },
                    { 79, "Chocolate rose makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 79, "assets/gallery/makeup/7.svg", true, "Chocolate Rose Glam", null },
                    { 80, "Enchanted lash lift inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 80, "assets/gallery/lashes/11.svg", true, "Enchanted Lash Lift", null },
                    { 81, "Golden barefoot pedicure inspiration", "pedicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pedicure", 81, "assets/gallery/pedicure/12.svg", true, "Golden Barefoot Glow", null },
                    { 82, "Rose muse makeup inspiration", "makeup", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Makeup", 82, "assets/gallery/makeup/13.svg", true, "Rose Muse Glam", null },
                    { 83, "Silk fan lashes inspiration", "lashes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lashes", 83, "assets/gallery/lashes/3.svg", true, "Silk Fan Lashes", null },
                    { 84, "Glossy fairy manicure inspiration", "manicure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manicure", 84, "assets/gallery/manicure/10.svg", true, "Glossy Fairy Nails", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GalleryImages_Category_DisplayOrder",
                table: "GalleryImages",
                columns: new[] { "Category", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_GalleryImages_ImageUrl",
                table: "GalleryImages",
                column: "ImageUrl",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GalleryImages");
        }
    }
}
