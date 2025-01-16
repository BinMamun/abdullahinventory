using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTaxesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Parcentage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Taxes",
                columns: new[] { "Id", "Name", "Parcentage" },
                values: new object[,]
                {
                    { new Guid("0ef5e8c0-ea9e-458d-b82f-9bd361f6eae5"), "VAT 5% (Garments, Crockeries, Toiletries,Raw Material Tax, etc)", 5.0 },
                    { new Guid("0fadbf4c-f8cf-4937-86a2-0ca33feb33f9"), "VAT 15% (Restaurants, Services, etc)", 15.0 },
                    { new Guid("24a44820-cff8-4509-aa16-a1f4c5bb0bd5"), "VAT 10% (Maintainance Service, Transport Service, etc.)", 10.0 },
                    { new Guid("39b76299-f549-4fc6-99a0-a15a607cb510"), "VAT 2.4% (Pharmaceuticals)", 2.3999999999999999 },
                    { new Guid("4db86802-9aea-4e7e-801a-b05a2463be39"), "VAT 13%", 13.0 },
                    { new Guid("81630e00-df5d-4c49-a335-aaeb0e44a4d4"), "VAT 2% (Petrolium, Builders, etc.)", 2.0 },
                    { new Guid("896e071d-f856-499b-aa07-2df1319fbcf1"), "VAT 7.5% (Paper, Auctioning goods, Own Branded Garments, etc.)", 7.5 },
                    { new Guid("cea979fd-08fd-4b38-b14a-b2518b481112"), "VAT 17.4%", 17.399999999999999 },
                    { new Guid("d7a519f6-6741-40e6-946c-405c9bed6225"), "VAT 7%", 7.0 },
                    { new Guid("f91d9f76-694c-42d5-afca-69252dc86eff"), "Tax Free", 0.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Taxes");
        }
    }
}
