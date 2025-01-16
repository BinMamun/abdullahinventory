using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateServiceAndTaxesTableRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TaxId",
                table: "Services",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_TaxId",
                table: "Services",
                column: "TaxId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Taxes_TaxId",
                table: "Services",
                column: "TaxId",
                principalTable: "Taxes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Taxes_TaxId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_TaxId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "TaxId",
                table: "Services");
        }
    }
}
