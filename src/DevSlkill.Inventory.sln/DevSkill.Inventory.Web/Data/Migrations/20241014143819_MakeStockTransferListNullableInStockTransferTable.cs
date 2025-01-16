using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeStockTransferListNullableInStockTransferTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransferItems_StockTransfers_StockTransferId",
                table: "StockTransferItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "StockTransferId",
                table: "StockTransferItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransferItems_StockTransfers_StockTransferId",
                table: "StockTransferItems",
                column: "StockTransferId",
                principalTable: "StockTransfers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransferItems_StockTransfers_StockTransferId",
                table: "StockTransferItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "StockTransferId",
                table: "StockTransferItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransferItems_StockTransfers_StockTransferId",
                table: "StockTransferItems",
                column: "StockTransferId",
                principalTable: "StockTransfers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
