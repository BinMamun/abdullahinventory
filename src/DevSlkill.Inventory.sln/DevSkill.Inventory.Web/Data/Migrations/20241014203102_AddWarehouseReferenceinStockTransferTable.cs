using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehouseReferenceinStockTransferTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "StockTransfers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_WarehouseId",
                table: "StockTransfers",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_Warehouses_WarehouseId",
                table: "StockTransfers",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_Warehouses_WarehouseId",
                table: "StockTransfers");

            migrationBuilder.DropIndex(
                name: "IX_StockTransfers_WarehouseId",
                table: "StockTransfers");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "StockTransfers");
        }
    }
}
