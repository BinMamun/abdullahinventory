using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateWarehouseRelationshipInStockTranferForShowingWarehouseNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_FromWarehouseId",
                table: "StockTransfers",
                column: "FromWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_ToWarehouseId",
                table: "StockTransfers",
                column: "ToWarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_Warehouses_FromWarehouseId",
                table: "StockTransfers",
                column: "FromWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_Warehouses_ToWarehouseId",
                table: "StockTransfers",
                column: "ToWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_Warehouses_FromWarehouseId",
                table: "StockTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_Warehouses_ToWarehouseId",
                table: "StockTransfers");

            migrationBuilder.DropIndex(
                name: "IX_StockTransfers_FromWarehouseId",
                table: "StockTransfers");

            migrationBuilder.DropIndex(
                name: "IX_StockTransfers_ToWarehouseId",
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
    }
}
