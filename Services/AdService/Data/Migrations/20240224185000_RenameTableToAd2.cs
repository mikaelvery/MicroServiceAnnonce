using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdService.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameTableToAd2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ads",
                keyColumn: "Id",
                keyValue: new Guid("d18af6ef-2cdb-4c90-aed3-c9567e400b56"));

            migrationBuilder.InsertData(
                table: "Ads",
                columns: new[] { "Id", "City", "IdAnnonce", "NumberOfRooms", "Price", "PublicationDate", "Status", "Title" },
                values: new object[] { new Guid("3cacad3e-aa78-444f-a5f3-080f312b6775"), "Montpellier", 1, 3, 300000m, new DateTime(2024, 2, 24, 19, 49, 59, 125, DateTimeKind.Local).AddTicks(2657), "Disponible", "Appartement centre ville" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ads",
                keyColumn: "Id",
                keyValue: new Guid("3cacad3e-aa78-444f-a5f3-080f312b6775"));

            migrationBuilder.InsertData(
                table: "Ads",
                columns: new[] { "Id", "City", "IdAnnonce", "NumberOfRooms", "Price", "PublicationDate", "Status", "Title" },
                values: new object[] { new Guid("d18af6ef-2cdb-4c90-aed3-c9567e400b56"), "Montpellier", 1, 3, 300000m, new DateTime(2024, 2, 24, 19, 41, 36, 59, DateTimeKind.Local).AddTicks(5110), "Disponible", "Appartement centre ville" });
        }
    }
}
