using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateNewSuperAdminWithAssignSuperAdminRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("a83f3b24-cb63-4ec4-bc80-ef4eaaba047d"), "85B8C5A1-9792-4326-9AE8-06376CCD638C", "Super Admin", "SUPER ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("41c1f025-c383-44bb-99d3-64a93dc932f9"), 0, "bcf92eec-277a-479a-9eec-6d4c9f9964b5", null, false, "Super", "Admin", false, null, null, "SUPERADMIN", "AQAAAAIAAYagAAAAEJvlP7iT7x94GkCsdqJRk0qGdGTywCXDjEP57/J0lodU+Z2mFSPoU2Trb20dkgYRQA==", null, false, "354FC657-CA80-458A-8719-ED65269683AD", false, "superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("a83f3b24-cb63-4ec4-bc80-ef4eaaba047d"), new Guid("41c1f025-c383-44bb-99d3-64a93dc932f9") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a83f3b24-cb63-4ec4-bc80-ef4eaaba047d"), new Guid("41c1f025-c383-44bb-99d3-64a93dc932f9") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a83f3b24-cb63-4ec4-bc80-ef4eaaba047d"));

            migrationBuilder.DeleteData(
               table: "AspNetUsers",
               keyColumn: "Id",
               keyValue: new Guid("41c1f025-c383-44bb-99d3-64a93dc932f9"),
               schema: null);
        }
    }
}
