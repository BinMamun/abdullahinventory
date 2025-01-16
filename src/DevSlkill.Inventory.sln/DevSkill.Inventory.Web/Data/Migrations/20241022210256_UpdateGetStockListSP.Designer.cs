﻿// <auto-generated />
using System;
using DevSkill.Inventory.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241022210256_UpdateGetStockListSP")]
    partial class UpdateGetStockListSP
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Barcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("BuyingPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("MeasurementUnitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("MinimumStockQuantity")
                        .HasColumnType("int");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("SellingPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("TaxId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("MeasurementUnitId");

                    b.HasIndex("TaxId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.ItemCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ItemCategories");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.ItemWarehouse", b =>
                {
                    b.Property<Guid>("ItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("WarehouseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("AsOfDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("CostPerUnit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<double?>("StockQuantity")
                        .HasColumnType("float");

                    b.HasKey("ItemId", "WarehouseId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("ItemWarehouses");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.MeasurementUnit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MeasurementUnits");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Service", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double?>("BuyingPrice")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("SellingPrice")
                        .HasColumnType("float");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TaxId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TaxId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockAdjustment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AdjustDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("StockAdjustmentReasonId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("WarehouseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("StockAdjustmentReasonId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("StockAdjustments");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockAdjustmentItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("AdjustedQuantity")
                        .HasColumnType("float");

                    b.Property<bool>("IsIncrease")
                        .HasColumnType("bit");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("StockAdjustmentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("StockAdjustmentId");

                    b.ToTable("StockAdjustmentItems");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockAdjustmentReason", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("StockAdjustmentReasons");

                    b.HasData(
                        new
                        {
                            Id = new Guid("164dc434-d1c9-4220-88f0-407517f7434c"),
                            Name = "Damaged Good"
                        },
                        new
                        {
                            Id = new Guid("1b256604-569c-4893-ba83-adf4f1b28e9b"),
                            Name = "Stock on fire"
                        },
                        new
                        {
                            Id = new Guid("ea2d5bb9-4dec-4c71-91de-713eadaaeab2"),
                            Name = "Stock Revaluation"
                        },
                        new
                        {
                            Id = new Guid("6484f4e0-15d3-4b48-a377-71d0b5b3c7bd"),
                            Name = "Sold"
                        },
                        new
                        {
                            Id = new Guid("e8314f84-b5e8-43d9-9522-eafee31e5d98"),
                            Name = "Purchased"
                        });
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockTransfer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FromWarehouseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ToWarehouseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("TransferDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FromWarehouseId");

                    b.HasIndex("ToWarehouseId");

                    b.ToTable("StockTransfers");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockTransferItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("StockTransferId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("TransferQuantity")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("StockTransferId");

                    b.ToTable("StockTransferItems");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Tax", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Parcentage")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Taxes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f91d9f76-694c-42d5-afca-69252dc86eff"),
                            Name = "Tax Free",
                            Parcentage = 0.0
                        },
                        new
                        {
                            Id = new Guid("24a44820-cff8-4509-aa16-a1f4c5bb0bd5"),
                            Name = "VAT 10% (Maintainance Service, Transport Service, etc.)",
                            Parcentage = 10.0
                        },
                        new
                        {
                            Id = new Guid("4db86802-9aea-4e7e-801a-b05a2463be39"),
                            Name = "VAT 13%",
                            Parcentage = 13.0
                        },
                        new
                        {
                            Id = new Guid("0fadbf4c-f8cf-4937-86a2-0ca33feb33f9"),
                            Name = "VAT 15% (Restaurants, Services, etc)",
                            Parcentage = 15.0
                        },
                        new
                        {
                            Id = new Guid("cea979fd-08fd-4b38-b14a-b2518b481112"),
                            Name = "VAT 17.4%",
                            Parcentage = 17.399999999999999
                        },
                        new
                        {
                            Id = new Guid("81630e00-df5d-4c49-a335-aaeb0e44a4d4"),
                            Name = "VAT 2% (Petrolium, Builders, etc.)",
                            Parcentage = 2.0
                        },
                        new
                        {
                            Id = new Guid("39b76299-f549-4fc6-99a0-a15a607cb510"),
                            Name = "VAT 2.4% (Pharmaceuticals)",
                            Parcentage = 2.3999999999999999
                        },
                        new
                        {
                            Id = new Guid("0ef5e8c0-ea9e-458d-b82f-9bd361f6eae5"),
                            Name = "VAT 5% (Garments, Crockeries, Toiletries,Raw Material Tax, etc)",
                            Parcentage = 5.0
                        },
                        new
                        {
                            Id = new Guid("d7a519f6-6741-40e6-946c-405c9bed6225"),
                            Name = "VAT 7%",
                            Parcentage = 7.0
                        },
                        new
                        {
                            Id = new Guid("896e071d-f856-499b-aa07-2df1319fbcf1"),
                            Name = "VAT 7.5% (Paper, Auctioning goods, Own Branded Garments, etc.)",
                            Parcentage = 7.5
                        });
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Warehouse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Warehouses");

                    b.HasData(
                        new
                        {
                            Id = new Guid("9bf404e5-ec2c-4472-be10-e80a6e4de22f"),
                            Name = "Main Warehouse"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Item", b =>
                {
                    b.HasOne("DevSkill.Inventory.Domain.Entities.ItemCategory", "Category")
                        .WithMany("Items")
                        .HasForeignKey("CategoryId");

                    b.HasOne("DevSkill.Inventory.Domain.Entities.MeasurementUnit", "MeasurementUnit")
                        .WithMany()
                        .HasForeignKey("MeasurementUnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevSkill.Inventory.Domain.Entities.Tax", "Tax")
                        .WithMany()
                        .HasForeignKey("TaxId");

                    b.Navigation("Category");

                    b.Navigation("MeasurementUnit");

                    b.Navigation("Tax");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.ItemWarehouse", b =>
                {
                    b.HasOne("DevSkill.Inventory.Domain.Entities.Item", "Item")
                        .WithMany("ItemWarehouses")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevSkill.Inventory.Domain.Entities.Warehouse", "Warehouse")
                        .WithMany("ItemWarehouses")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Service", b =>
                {
                    b.HasOne("DevSkill.Inventory.Domain.Entities.Tax", "Tax")
                        .WithMany()
                        .HasForeignKey("TaxId");

                    b.Navigation("Tax");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockAdjustment", b =>
                {
                    b.HasOne("DevSkill.Inventory.Domain.Entities.StockAdjustmentReason", "StockAdjustmentReason")
                        .WithMany()
                        .HasForeignKey("StockAdjustmentReasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevSkill.Inventory.Domain.Entities.Warehouse", "Warehouse")
                        .WithMany()
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StockAdjustmentReason");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockAdjustmentItem", b =>
                {
                    b.HasOne("DevSkill.Inventory.Domain.Entities.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevSkill.Inventory.Domain.Entities.StockAdjustment", "StockAdjustment")
                        .WithMany("StockAdjustmentItems")
                        .HasForeignKey("StockAdjustmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("StockAdjustment");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockTransfer", b =>
                {
                    b.HasOne("DevSkill.Inventory.Domain.Entities.Warehouse", "FromWarehouse")
                        .WithMany()
                        .HasForeignKey("FromWarehouseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DevSkill.Inventory.Domain.Entities.Warehouse", "ToWarehouse")
                        .WithMany()
                        .HasForeignKey("ToWarehouseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FromWarehouse");

                    b.Navigation("ToWarehouse");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockTransferItem", b =>
                {
                    b.HasOne("DevSkill.Inventory.Domain.Entities.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevSkill.Inventory.Domain.Entities.StockTransfer", "StockTransfer")
                        .WithMany("StockTransferItems")
                        .HasForeignKey("StockTransferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("StockTransfer");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Item", b =>
                {
                    b.Navigation("ItemWarehouses");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.ItemCategory", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockAdjustment", b =>
                {
                    b.Navigation("StockAdjustmentItems");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockTransfer", b =>
                {
                    b.Navigation("StockTransferItems");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Warehouse", b =>
                {
                    b.Navigation("ItemWarehouses");
                });
#pragma warning restore 612, 618
        }
    }
}
