using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public class StockAdjustmentManagementService : IStockAdjustmentManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public StockAdjustmentManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public async Task CreateStockAdjustmentAsync(StockAdjustment stockAdjustment, List<StockAdjustmentItem> stockAdjustmentItems)
        {
            await UpdateItemWarehouse(stockAdjustmentItems, stockAdjustment.WarehouseId, stockAdjustment.AdjustDate);
            await _inventoryUnitOfWork.StockAdjustmentRepository.AddAsync(stockAdjustment);
            await _inventoryUnitOfWork.SaveAsync();
        }

        public async Task CreateStockAdjustmentReasonAsync(StockAdjustmentReason reason)
        {
            await _inventoryUnitOfWork.StockAdjustmentReasonRepository.AddAsync(reason);
            await _inventoryUnitOfWork.SaveAsync();
        }

        public async Task DeleteStockAdjustmentAsync(Guid id)
        {
            var stockAdjustment = await _inventoryUnitOfWork.StockAdjustmentRepository
                        .GetStockAdjustmentByIdWithItemsAsync(id);

            var stockAdjustmentItems = stockAdjustment?.StockAdjustmentItems?.ToList();

            if(stockAdjustmentItems?.Count > 0)
            {
                foreach (var item in stockAdjustmentItems)
                {
                    var itemWarehouse = await _inventoryUnitOfWork.ItemWarehouseRepository
                        .GetByIdAsync(item.ItemId, stockAdjustment.WarehouseId);

                    if (itemWarehouse != null)
                    {
                        if (item.IsIncrease)
                        {
                            itemWarehouse.StockQuantity -= item.AdjustedQuantity;
                            await _inventoryUnitOfWork.ItemWarehouseRepository.EditAsync(itemWarehouse);
                        }
                        else
                        {
                            itemWarehouse.StockQuantity += item.AdjustedQuantity;
                            await _inventoryUnitOfWork.ItemWarehouseRepository.EditAsync(itemWarehouse);
                        }
                    }
                    else
                    {
                        throw new Exception("Warehouse not found");
                    }
                }
            }
            
            await _inventoryUnitOfWork.StockAdjustmentItemRepository
                                        .RemoveRangeAsync(stockAdjustmentItems);
            await _inventoryUnitOfWork.StockAdjustmentRepository.RemoveAsync(stockAdjustment);

            await _inventoryUnitOfWork.SaveAsync();
        }

        public async Task<IList<StockAdjustmentReason>> GetStockAdjustmentReasonsAsync()
        {
            return await _inventoryUnitOfWork.StockAdjustmentReasonRepository.GetAllAsync();
        }

        public async Task<(IList<StockAdjustment> data, int total, int totalDisplay)> GetStockAdjustmentsAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return await _inventoryUnitOfWork.StockAdjustmentRepository
                    .GetPagedStockAdjustmentListAsync(pageIndex, pageSize, search, order);
        }

        private async Task UpdateItemWarehouse(List<StockAdjustmentItem> stockAdjustmentItems, Guid warehouseId, DateTime adjustDate)
        {
            foreach (var item in stockAdjustmentItems)
            {
                var itemWarehouse = await _inventoryUnitOfWork.ItemWarehouseRepository
                    .GetByIdAsync(item.ItemId, warehouseId);

                if (itemWarehouse != null)
                {
                    if (item.IsIncrease)
                    {
                        itemWarehouse.StockQuantity += item.AdjustedQuantity;
                        await _inventoryUnitOfWork.ItemWarehouseRepository.EditAsync(itemWarehouse);
                    }
                    else
                    {
                        itemWarehouse.StockQuantity -= item.AdjustedQuantity;
                        await _inventoryUnitOfWork.ItemWarehouseRepository.EditAsync(itemWarehouse);
                    }
                }
                else
                {
                    var itemInfo = await _inventoryUnitOfWork.ItemRepository
                                        .GetItemByIdAsync(item.ItemId);

                    var newItemWarehouse = new ItemWarehouse
                    {
                        ItemId = item.ItemId,
                        WarehouseId = warehouseId,
                        StockQuantity = 0,
                        CostPerUnit = itemInfo.BuyingPrice,
                        AsOfDate = adjustDate
                    };
                    newItemWarehouse.StockQuantity = item.IsIncrease == true ? (newItemWarehouse.StockQuantity + item.AdjustedQuantity) : (newItemWarehouse.StockQuantity - item.AdjustedQuantity);

                    await _inventoryUnitOfWork.ItemWarehouseRepository
                        .AddAsync(newItemWarehouse);
                }
            }
        }

    }//Class
}//Namespace
