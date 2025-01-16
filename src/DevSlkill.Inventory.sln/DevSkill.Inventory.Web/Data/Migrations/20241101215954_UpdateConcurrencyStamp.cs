using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConcurrencyStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a83f3b24-cb63-4ec4-bc80-ef4eaaba047d"),
                column: "ConcurrencyStamp",
                value: "85B8C5A1-9792-4326-9AE8-06376CCD638C");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("41c1f025-c383-44bb-99d3-64a93dc932f9"),
                column: "ConcurrencyStamp",
                value: "2B157DA1-B4A6-4CAD-8CCE-F5BD6CB1B9AB");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a83f3b24-cb63-4ec4-bc80-ef4eaaba047d"),
                column: "ConcurrencyStamp",
                value: "638661157900794457");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("41c1f025-c383-44bb-99d3-64a93dc932f9"),
                column: "ConcurrencyStamp",
                value: "b2caf14a-e4b1-47a3-aeda-0f39259213c6");
        }
    }
}
