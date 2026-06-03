using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGalleryImageFavorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GalleryImageFavorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GalleryImageId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalleryImageFavorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GalleryImageFavorites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GalleryImageFavorites_GalleryImages_GalleryImageId",
                        column: x => x.GalleryImageId,
                        principalTable: "GalleryImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GalleryImageFavorites_GalleryImageId",
                table: "GalleryImageFavorites",
                column: "GalleryImageId");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryImageFavorites_UserId",
                table: "GalleryImageFavorites",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryImageFavorites_UserId_GalleryImageId",
                table: "GalleryImageFavorites",
                columns: new[] { "UserId", "GalleryImageId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GalleryImageFavorites");
        }
    }
}
