using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedStockAdjustmentReasonsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "StockAdjustmentReasons",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("164dc434-d1c9-4220-88f0-407517f7434c"), "Damaged Good" },
                    { new Guid("1b256604-569c-4893-ba83-adf4f1b28e9b"), "Stock on fire" },
                    { new Guid("6484f4e0-15d3-4b48-a377-71d0b5b3c7bd"), "Sold" },
                    { new Guid("e8314f84-b5e8-43d9-9522-eafee31e5d98"), "Purchased" },
                    { new Guid("ea2d5bb9-4dec-4c71-91de-713eadaaeab2"), "Stock Revaluation" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StockAdjustmentReasons",
                keyColumn: "Id",
                keyValue: new Guid("164dc434-d1c9-4220-88f0-407517f7434c"));

            migrationBuilder.DeleteData(
                table: "StockAdjustmentReasons",
                keyColumn: "Id",
                keyValue: new Guid("1b256604-569c-4893-ba83-adf4f1b28e9b"));

            migrationBuilder.DeleteData(
                table: "StockAdjustmentReasons",
                keyColumn: "Id",
                keyValue: new Guid("6484f4e0-15d3-4b48-a377-71d0b5b3c7bd"));

            migrationBuilder.DeleteData(
                table: "StockAdjustmentReasons",
                keyColumn: "Id",
                keyValue: new Guid("e8314f84-b5e8-43d9-9522-eafee31e5d98"));

            migrationBuilder.DeleteData(
                table: "StockAdjustmentReasons",
                keyColumn: "Id",
                keyValue: new Guid("ea2d5bb9-4dec-4c71-91de-713eadaaeab2"));
        }
    }
}
