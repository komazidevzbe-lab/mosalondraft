using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceBookingFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingReference = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BookingMode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ClientFullName = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    ClientEmailAddress = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    ClientPhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PreferredContactMethod = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    TotalDurationMinutes = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BookingStatus = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalonServiceLengthOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalonServiceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    PriceAddOn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalonServiceLengthOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalonServiceLengthOptions_SalonServices_SalonServiceId",
                        column: x => x.SalonServiceId,
                        principalTable: "SalonServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalonServiceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalonServiceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalonServiceTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalonServiceTypes_SalonServices_SalonServiceId",
                        column: x => x.SalonServiceId,
                        principalTable: "SalonServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    PaymentProvider = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MerchantReference = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    GatewayPaymentId = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    GatewayTransactionId = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    RawGatewayResponse = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingPayments_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingServiceItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    SalonServiceId = table.Column<int>(type: "int", nullable: false),
                    SalonServiceTypeId = table.Column<int>(type: "int", nullable: false),
                    SalonServiceLengthOptionId = table.Column<int>(type: "int", nullable: true),
                    AppointmentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    ServiceNameSnapshot = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    ServiceTypeNameSnapshot = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    LengthNameSnapshot = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    BasePriceSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LengthAddOnPriceSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationMinutesSnapshot = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReferenceImageType = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    GalleryImageId = table.Column<int>(type: "int", nullable: true),
                    UploadedReferenceImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReferenceImagePreviewUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingServiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingServiceItems_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingServiceItems_GalleryImages_GalleryImageId",
                        column: x => x.GalleryImageId,
                        principalTable: "GalleryImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BookingServiceItems_SalonServiceLengthOptions_SalonServiceLengthOptionId",
                        column: x => x.SalonServiceLengthOptionId,
                        principalTable: "SalonServiceLengthOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookingServiceItems_SalonServiceTypes_SalonServiceTypeId",
                        column: x => x.SalonServiceTypeId,
                        principalTable: "SalonServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookingServiceItems_SalonServices_SalonServiceId",
                        column: x => x.SalonServiceId,
                        principalTable: "SalonServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayments_BookingId",
                table: "BookingPayments",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayments_CreatedAt",
                table: "BookingPayments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayments_MerchantReference",
                table: "BookingPayments",
                column: "MerchantReference");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayments_PaymentStatus",
                table: "BookingPayments",
                column: "PaymentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BookingReference",
                table: "Bookings",
                column: "BookingReference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BookingStatus",
                table: "Bookings",
                column: "BookingStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CreatedAt",
                table: "Bookings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PaymentStatus",
                table: "Bookings",
                column: "PaymentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingServiceItems_AppointmentDate",
                table: "BookingServiceItems",
                column: "AppointmentDate");

            migrationBuilder.CreateIndex(
                name: "IX_BookingServiceItems_AppointmentDate_StartTime_EndTime",
                table: "BookingServiceItems",
                columns: new[] { "AppointmentDate", "StartTime", "EndTime" });

            migrationBuilder.CreateIndex(
                name: "IX_BookingServiceItems_BookingId",
                table: "BookingServiceItems",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingServiceItems_GalleryImageId",
                table: "BookingServiceItems",
                column: "GalleryImageId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingServiceItems_SalonServiceId",
                table: "BookingServiceItems",
                column: "SalonServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingServiceItems_SalonServiceLengthOptionId",
                table: "BookingServiceItems",
                column: "SalonServiceLengthOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingServiceItems_SalonServiceTypeId",
                table: "BookingServiceItems",
                column: "SalonServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalonServiceLengthOptions_SalonServiceId_DisplayOrder",
                table: "SalonServiceLengthOptions",
                columns: new[] { "SalonServiceId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_SalonServiceLengthOptions_SalonServiceId_Name",
                table: "SalonServiceLengthOptions",
                columns: new[] { "SalonServiceId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalonServiceTypes_SalonServiceId_DisplayOrder",
                table: "SalonServiceTypes",
                columns: new[] { "SalonServiceId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_SalonServiceTypes_SalonServiceId_Name",
                table: "SalonServiceTypes",
                columns: new[] { "SalonServiceId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingPayments");

            migrationBuilder.DropTable(
                name: "BookingServiceItems");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "SalonServiceLengthOptions");

            migrationBuilder.DropTable(
                name: "SalonServiceTypes");
        }
    }
}
