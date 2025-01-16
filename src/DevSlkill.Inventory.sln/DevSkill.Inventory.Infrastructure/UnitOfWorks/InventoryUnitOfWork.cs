using DevSkill.Inventory.Application;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Inventory.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.UnitOfWorks
{
    public class InventoryUnitOfWork : UnitOfWork, IInventoryUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; private set; }
        public IMeasurementUnitRepository MeasurementUnitRepository { get; private set; }
        public ITaxRepository TaxRepository { get; private set; }
        public IServiceRepository ServiceRepository { get; private set; }
        public IItemRepository ItemRepository { get; private set; }
        public IWarehouseRepository WarehouseRepository { get; private set; }
        public IItemWarehouseRepository ItemWarehouseRepository { get; private set; }
        public IStockTransferRepository StockTransferRepository { get; private set; }
        public IStockTransferItemRepository StockTransferItemRepository { get; private set; }
        public IStockAdjustmentReasonRepository StockAdjustmentReasonRepository { get; private set; }
        public IStockAdjustmentItemRepository StockAdjustmentItemRepository { get; private set; }
        public IStockAdjustmentRepository StockAdjustmentRepository { get; private set; }

        public InventoryUnitOfWork(
                ApplicationDbContext dbContext,
                ICategoryRepository categoryRepository,
                IMeasurementUnitRepository measurementUnitRepository,
                ITaxRepository taxRepository,
                IServiceRepository serviceRepository,
                IItemRepository itemRepository,
                IWarehouseRepository warehouseRepository,
                IItemWarehouseRepository itemWarehouseRepository,
                IStockTransferRepository stockTransferRepository,
                IStockTransferItemRepository stockTransferItemRepository,
                IStockAdjustmentReasonRepository stockAdjustmentReasonRepository,
                IStockAdjustmentItemRepository stockAdjustmentItemRepository,
                IStockAdjustmentRepository stockAdjustmentRepository
                ) : base(dbContext)
        {
            CategoryRepository = categoryRepository;
            MeasurementUnitRepository = measurementUnitRepository;
            TaxRepository = taxRepository;
            ServiceRepository = serviceRepository;
            ItemRepository = itemRepository;
            WarehouseRepository = warehouseRepository;
            ItemWarehouseRepository = itemWarehouseRepository;
            StockTransferRepository = stockTransferRepository;
            StockTransferItemRepository = stockTransferItemRepository;
            StockAdjustmentReasonRepository = stockAdjustmentReasonRepository;
            StockAdjustmentItemRepository = stockAdjustmentItemRepository;
            StockAdjustmentRepository = stockAdjustmentRepository;
        }

        public async Task<(IList<StockListDto> data, int total, int totalDisplay)> GetPagedStockListAsync(int pageIndex, int pageSize, StockListSearchDto search, string? order)
        {
            var procedureName = "GetStockList";
            var result = await SqlUtility.QueryWithStoredProcedureAsync<StockListDto>
                (procedureName,
                new Dictionary<string, object>
                {
                    { "PageIndex", pageIndex },
                    { "PageSize", pageSize },
                    { "OrderBy", order },
                    { "ItemName", string.IsNullOrEmpty(search.ItemName) ?
                        null : search.ItemName},
                    { "Barcode", string.IsNullOrEmpty(search.Barcode) ?
                        null : search.Barcode},
                    { "CategoryId", string.IsNullOrEmpty(search.CategoryId) ?
                        null: Guid.Parse(search.CategoryId)},
                    { "WarehouseId", string.IsNullOrEmpty(search.WarehouseId) ?
                        null: Guid.Parse(search.WarehouseId)},
                    { "StockGreaterThanZero", search.StockGreaterThanZero },
                    { "BelowMinimumStock", search.BelowMinimumStock }
                },
                new Dictionary<string, Type>
                {
                    { "Total", typeof(int) },
                    { "TotalDisplay", typeof(int) }
                });

            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }
    }//Class
}//Namespace
