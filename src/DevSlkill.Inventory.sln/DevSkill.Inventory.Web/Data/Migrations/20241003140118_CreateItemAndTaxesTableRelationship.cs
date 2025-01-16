using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateItemAndTaxesTableRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TaxId",
                table: "Items",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_TaxId",
                table: "Items",
                column: "TaxId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Taxes_TaxId",
                table: "Items",
                column: "TaxId",
                principalTable: "Taxes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Taxes_TaxId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_TaxId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "TaxId",
                table: "Items");
        }
    }
}
