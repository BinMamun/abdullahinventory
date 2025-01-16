using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateItemAndMeasurementUnitRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MeasurementUnitId",
                table: "Items",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Items_MeasurementUnitId",
                table: "Items",
                column: "MeasurementUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_MeasurementUnits_MeasurementUnitId",
                table: "Items",
                column: "MeasurementUnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_MeasurementUnits_MeasurementUnitId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_MeasurementUnitId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitId",
                table: "Items");
        }
    }
}
