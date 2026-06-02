using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreClientsReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ClientReviews",
                columns: new[] { "Id", "AltText", "ClientName", "CreatedAt", "DisplayOrder", "ImageUrl", "IsApproved", "IsFeatured", "Location", "Rating", "ReviewText", "UpdatedAt" },
                values: new object[,]
                {
                    { 4, "Client review profile", "Thando K.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "assets/gallery/makeup/15.svg", true, true, "Kimberley, NC", 5, "The makeup finish was soft, clean, and exactly what I wanted for my event.", null },
                    { 5, "Client review profile", "Anele D.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, "assets/gallery/makeup/18.svg", true, true, "Durban, KZN", 5, "My manicure lasted beautifully and the whole appointment felt calm and professional.", null },
                    { 6, "Client review profile", "Mpho S.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, "assets/gallery/makeup/20.svg", true, true, "Pretoria, GP", 5, "The lash set was neat, comfortable, and gave me the exact natural glam I asked for.", null },
                    { 7, "Client review profile", "Zanele B.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, "assets/gallery/makeup/5.svg", true, true, "Gqeberha, EC", 5, "Beautiful service from start to finish. I left feeling pampered and confident.", null },
                    { 8, "Client review profile", "Naledi P.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8, "assets/gallery/makeup/9.svg", true, true, "Polokwane, LP", 5, "The team listened carefully and delivered a clean, elegant look that suited me perfectly.", null },
                    { 9, "Client review profile", "Refilwe T.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, "assets/gallery/makeup/11.svg", true, true, "Rustenburg, NW", 5, "I loved the attention to detail. My nails looked polished, feminine, and flawless.", null },
                    { 10, "Client review profile", "Boitumelo N.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10, "assets/gallery/makeup/14.svg", true, true, "Bloemfontein, FS", 5, "The pedicure was relaxing and my feet looked fresh for weeks. I would definitely return.", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ClientReviews",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ClientReviews",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ClientReviews",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ClientReviews",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ClientReviews",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ClientReviews",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ClientReviews",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
