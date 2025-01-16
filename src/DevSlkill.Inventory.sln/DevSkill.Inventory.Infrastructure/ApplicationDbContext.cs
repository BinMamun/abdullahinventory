using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Xml;

namespace DevSkill.Inventory.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext
                                        <ApplicationUser, ApplicationRole, Guid,
                                        ApplicationUserClaim, ApplicationUserRole,
                                        ApplicationUserLogin, ApplicationRoleClaim,
                                        ApplicationUserToken>
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;

        public ApplicationDbContext(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString,
                    x => x.MigrationsAssembly(_migrationAssembly));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Tax>().HasData
                (
                    new Tax
                    {
                        Id = Guid.Parse("F91D9F76-694C-42D5-AFCA-69252DC86EFF"),
                        Name = "Tax Free",
                        Parcentage = 0
                    },
                    new Tax
                    {
                        Id = Guid.Parse("24A44820-CFF8-4509-AA16-A1F4C5BB0BD5"),
                        Name = "VAT 10% (Maintainance Service, Transport Service, etc.)",
                        Parcentage = 10
                    },
                    new Tax
                    {
                        Id = Guid.Parse("4DB86802-9AEA-4E7E-801A-B05A2463BE39"),
                        Name = "VAT 13%",
                        Parcentage = 13
                    },
                    new Tax
                    {
                        Id = Guid.Parse("0FADBF4C-F8CF-4937-86A2-0CA33FEB33F9"),
                        Name = "VAT 15% (Restaurants, Services, etc)",
                        Parcentage = 15
                    },
                    new Tax
                    {
                        Id = Guid.Parse("CEA979FD-08FD-4B38-B14A-B2518B481112"),
                        Name = "VAT 17.4%",
                        Parcentage = 17.4
                    },
                    new Tax
                    {
                        Id = Guid.Parse("81630E00-DF5D-4C49-A335-AAEB0E44A4D4"),
                        Name = "VAT 2% (Petrolium, Builders, etc.)",
                        Parcentage = 2
                    },
                    new Tax
                    {
                        Id = Guid.Parse("39B76299-F549-4FC6-99A0-A15A607CB510"),
                        Name = "VAT 2.4% (Pharmaceuticals)",
                        Parcentage = 2.4
                    },
                    new Tax
                    {
                        Id = Guid.Parse("0EF5E8C0-EA9E-458D-B82F-9BD361F6EAE5"),
                        Name = "VAT 5% (Garments, Crockeries, Toiletries,Raw Material Tax, etc)",
                        Parcentage = 5
                    },
                    new Tax
                    {
                        Id = Guid.Parse("D7A519F6-6741-40E6-946C-405C9BED6225"),
                        Name = "VAT 7%",
                        Parcentage = 7
                    },
                    new Tax
                    {
                        Id = Guid.Parse("896E071D-F856-499B-AA07-2DF1319FBCF1"),
                        Name = "VAT 7.5% (Paper, Auctioning goods, Own Branded Garments, etc.)",
                        Parcentage = 7.5
                    }
                );

            builder.Entity<Service>()
                .HasOne(x => x.Tax)
                .WithMany()
                .HasForeignKey(y => y.TaxId);

            builder.Entity<Item>().Property(x => x.IsActive).HasColumnType("bit");

            builder.Entity<Item>()
                .HasOne(x => x.Tax)
                .WithMany()
                .HasForeignKey(y => y.TaxId);

            builder.Entity<Item>()
                .HasOne(x => x.Category)
                .WithMany(c => c.Items)
                .HasForeignKey(y => y.CategoryId);

            builder.Entity<Item>()
                .HasOne(x => x.MeasurementUnit)
                .WithMany()
                .HasForeignKey(y => y.MeasurementUnitId);

            builder.Entity<Warehouse>()
                .HasData
                (
                    new Warehouse
                    {
                        Id = Guid.Parse("{9BF404E5-EC2C-4472-BE10-E80A6E4DE22F}"),
                        Name = "Main Warehouse"
                    }
                );

            builder.Entity<ItemWarehouse>()
               .HasKey(x => new { x.ItemId, x.WarehouseId });

            builder.Entity<ItemWarehouse>()
                .HasOne(x => x.Item)
                .WithMany(y => y.ItemWarehouses)
                .HasForeignKey(z => z.ItemId);

            builder.Entity<ItemWarehouse>()
                .HasOne(x => x.Warehouse)
                .WithMany(y => y.ItemWarehouses)
                .HasForeignKey(z => z.WarehouseId);

            builder.Entity<StockTransfer>()
                .HasOne(st => st.FromWarehouse)
                .WithMany()
                .HasForeignKey(st => st.FromWarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StockTransfer>()
                .HasOne(st => st.ToWarehouse)
                .WithMany()
                .HasForeignKey(st => st.ToWarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StockAdjustmentReason>()
                .HasData
                (
                    new StockAdjustmentReason
                    {
                        Id = Guid.Parse("164DC434-D1C9-4220-88F0-407517F7434C"),
                        Name = "Damaged Good"
                    },
                    new StockAdjustmentReason
                    {
                        Id = Guid.Parse("{1B256604-569C-4893-BA83-ADF4F1B28E9B}"),
                        Name = "Stock on fire"
                    },
                    new StockAdjustmentReason
                    {
                        Id = Guid.Parse("{EA2D5BB9-4DEC-4C71-91DE-713EADAAEAB2}"),
                        Name = "Stock Revaluation"
                    },
                    new StockAdjustmentReason
                    {
                        Id = Guid.Parse("{6484F4E0-15D3-4B48-A377-71D0B5B3C7BD}"),
                        Name = "Sold"
                    },
                    new StockAdjustmentReason
                    {
                        Id = Guid.Parse("{E8314F84-B5E8-43D9-9522-EAFEE31E5D98}"),
                        Name = "Purchased"
                    }
                );

            builder.Entity<ApplicationUser>()
                .HasData
                (
                    new ApplicationUser
                    {
                        Id= Guid.Parse("41c1f025-c383-44bb-99d3-64a93dc932f9"),
                        UserName = "superadmin",
                        NormalizedUserName = "SUPERADMIN",
                        PasswordHash = "AQAAAAIAAYagAAAAEJvlP7iT7x94GkCsdqJRk0qGdGTywCXDjEP57/J0lodU+Z2mFSPoU2Trb20dkgYRQA==",
                        FirstName = "Super",
                        LastName = "Admin",
                        SecurityStamp = "354FC657-CA80-458A-8719-ED65269683AD",
                        ConcurrencyStamp = "2B157DA1-B4A6-4CAD-8CCE-F5BD6CB1B9AB"
                    }
                );

            builder.Entity<ApplicationRole>()
                .HasData
                (
                    new ApplicationRole
                    {
                        Id = Guid.Parse("a83f3b24-cb63-4ec4-bc80-ef4eaaba047d"),
                        Name = "Super Admin",
                        NormalizedName = "SUPER ADMIN",
                        ConcurrencyStamp = "85B8C5A1-9792-4326-9AE8-06376CCD638C"
                    }
                );

            builder.Entity<ApplicationUserRole>()
                .HasData
                (
                    new ApplicationUserRole
                    {
                        RoleId = Guid.Parse("a83f3b24-cb63-4ec4-bc80-ef4eaaba047d"),
                        UserId = Guid.Parse("41c1f025-c383-44bb-99d3-64a93dc932f9")
                    }
                );
            

            base.OnModelCreating(builder);
        }

        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<ItemWarehouse> ItemWarehouses { get; set; }
        public DbSet<StockTransfer> StockTransfers { get; set; }
        public DbSet<StockTransferItem> StockTransferItems { get; set; }
        public DbSet<StockAdjustmentReason> StockAdjustmentReasons { get; set; }
        public DbSet<StockAdjustmentItem> StockAdjustmentItems { get; set; }
        public DbSet<StockAdjustment> StockAdjustments { get; set; }
    }
}