using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailOptimizationPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddSmartTvSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 1, 21, 50, 54, 954, DateTimeKind.Local).AddTicks(2093));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 1, 21, 50, 54, 954, DateTimeKind.Local).AddTicks(2138));

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Category", "CreatedAt", "Price", "ProductName", "ReorderLevel", "StockQuantity" },
                values: new object[] { 3, "TV", new DateTime(2026, 6, 1, 21, 50, 54, 954, DateTimeKind.Local).AddTicks(2140), 10000m, "Smart TV", 5, 19 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 31, 15, 35, 12, 714, DateTimeKind.Local).AddTicks(5446));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 31, 15, 35, 12, 714, DateTimeKind.Local).AddTicks(5458));
        }
    }
}
