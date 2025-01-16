using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateRelationshipBetweenWarehouseAndStockTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "StockTransferId",
                table: "Warehouses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: new Guid("9bf404e5-ec2c-4472-be10-e80a6e4de22f"),
                column: "StockTransferId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_StockTransferId",
                table: "Warehouses",
                column: "StockTransferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_StockTransfers_StockTransferId",
                table: "Warehouses",
                column: "StockTransferId",
                principalTable: "StockTransfers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_StockTransfers_StockTransferId",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_StockTransferId",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "StockTransferId",
                table: "Warehouses");

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
    }
}
