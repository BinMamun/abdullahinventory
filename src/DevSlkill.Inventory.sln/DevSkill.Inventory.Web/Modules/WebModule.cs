using Autofac;
using DevSkill.Inventory.Application;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.RepositoryContracts;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Infrastructure.Repositories;
using DevSkill.Inventory.Infrastructure.UnitOfWorks;
using DevSkill.Inventory.Infrastructure.Utilities;

namespace DevSkill.Inventory.Web.Modules
{
    public class WebModule(string connectionString, string migrationAssembly) : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().AsSelf()
                .WithParameter("connectionString", connectionString)
                .WithParameter("migrationAssembly", migrationAssembly)
                .InstancePerLifetimeScope();

            builder.RegisterType<InventoryUnitOfWork>()
                .As<IInventoryUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CategoryManagementService>()
                .As<ICategoryManagementService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CategoryRepository>()
                .As<ICategoryRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MeasurementUnitManagementService>()
                .As<IMeasurementUnitManagementService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MeasurementUnitRepository>()
                .As<IMeasurementUnitRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TaxManagementService>()
                .As<ITaxManagementService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TaxReporsitory>()
                .As<ITaxRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ServiceManagementService>()
               .As<IServiceManagementService>()
               .InstancePerLifetimeScope();

            builder.RegisterType<ServiceRepository>()
                .As<IServiceRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ItemManagementService>()
               .As<IItemManagementService>()
               .InstancePerLifetimeScope();

            builder.RegisterType<ItemRepository>()
              .As<IItemRepository>()
              .InstancePerLifetimeScope();

            builder.RegisterType<ImageServiceUtility>()
              .As<IImageServiceUtility>()
              .InstancePerLifetimeScope();

            builder.RegisterType<WarehouseManagementService>()
               .As<IWarehouseManagementService>()
               .InstancePerLifetimeScope();

            builder.RegisterType<WarehouseRepository>()
              .As<IWarehouseRepository>()
              .InstancePerLifetimeScope();

            builder.RegisterType<ItemWarehouseRepository>()
             .As<IItemWarehouseRepository>()
             .InstancePerLifetimeScope();

            builder.RegisterType<StockTransferRepository>()
              .As<IStockTransferRepository>()
              .InstancePerLifetimeScope();

            builder.RegisterType<StockTransferManagementService>()
             .As<IStockTransferManagementService>()
             .InstancePerLifetimeScope();

            builder.RegisterType<StockTransferItemRepository>()
              .As<IStockTransferItemRepository>()
              .InstancePerLifetimeScope();

            builder.RegisterType<StockAdjustmentReasonRepository>()
              .As<IStockAdjustmentReasonRepository>()
              .InstancePerLifetimeScope();

            builder.RegisterType<StockAdjustmentItemRepository>()
              .As<IStockAdjustmentItemRepository>()
              .InstancePerLifetimeScope();

            builder.RegisterType<StockAdjustmentRepository>()
              .As<IStockAdjustmentRepository>()
              .InstancePerLifetimeScope();

            builder.RegisterType<StockAdjustmentManagementService>()
             .As<IStockAdjustmentManagementService>()
             .InstancePerLifetimeScope();

            builder.RegisterType<DisplayStockListService>()
             .As<IDisplayStockListService>()
             .InstancePerLifetimeScope();

            builder.RegisterType<RedirectIfLoggedin>().AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}
