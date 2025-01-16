using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public class StockTransferManagementService : IStockTransferManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public StockTransferManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }
        public async Task CreateStockTransferAsync(StockTransfer stockTransfer, List<StockTransferItem> stockTransferItems)
        {
            await UpdateItemWarehouseAsync(stockTransferItems, stockTransfer.TransferDate, stockTransfer.FromWarehouseId, stockTransfer.ToWarehouseId);
            await _inventoryUnitOfWork.StockTransferRepository.AddAsync(stockTransfer);
            await _inventoryUnitOfWork.SaveAsync();
        }

        public async Task DeleteStockTransferAsync(Guid id)
        {
            var stockTransfer = await _inventoryUnitOfWork.StockTransferRepository
                                        .GetStockTransferWithItemsAsync(id);

            var stockTransferItems = stockTransfer.StockTransferItems.ToList();

            foreach (var item in stockTransferItems)
            {
                var itemWarehouse = await _inventoryUnitOfWork.ItemWarehouseRepository
                    .GetByIdAsync(item.ItemId, stockTransfer.FromWarehouseId);

                if (itemWarehouse != null)
                {
                    itemWarehouse.StockQuantity += item.TransferQuantity;
                    await _inventoryUnitOfWork.ItemWarehouseRepository.EditAsync(itemWarehouse);

                    var toItemWarehouse = await _inventoryUnitOfWork.ItemWarehouseRepository
                    .GetByIdAsync(item.ItemId, stockTransfer.ToWarehouseId);

                    if (toItemWarehouse != null)
                    {
                        toItemWarehouse.StockQuantity -= item.TransferQuantity;
                        await _inventoryUnitOfWork.ItemWarehouseRepository
                            .EditAsync(toItemWarehouse);
                    }
                }
            }
            await _inventoryUnitOfWork.StockTransferItemRepository
                    .RemoveRangeAsync(stockTransferItems);
            await _inventoryUnitOfWork.StockTransferRepository.RemoveAsync(stockTransfer);
            await _inventoryUnitOfWork.SaveAsync();
        }

        public async Task<StockTransfer> GetStockTransferItemsAsync(Guid id)
        {
            return await _inventoryUnitOfWork.StockTransferRepository
                            .GetStockTransferWithItemsAsync(id);
        }

        public async Task<(IList<StockTransfer> data, int total, int totalDisplay)> GetStockTransfersAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return await _inventoryUnitOfWork.StockTransferRepository.GetPagedStockTransfersAsync(pageIndex, pageSize, search, order);
        }

        private async Task UpdateItemWarehouseAsync(List<StockTransferItem> stockTransferItems, DateTime transferDate, Guid fromWarehouseId, Guid toWarehouseId)
        {
            foreach (var item in stockTransferItems)
            {
                var itemWarehouse = await _inventoryUnitOfWork.ItemWarehouseRepository
                    .GetByIdAsync(item.ItemId, fromWarehouseId);

                if(itemWarehouse != null)
                {
                    if(itemWarehouse.StockQuantity >= item.TransferQuantity)
                    {
                        itemWarehouse.StockQuantity -= item.TransferQuantity;
                        await _inventoryUnitOfWork.ItemWarehouseRepository.EditAsync(itemWarehouse);

                        var toItemWarehouse = await _inventoryUnitOfWork.ItemWarehouseRepository
                        .GetByIdAsync(item.ItemId, toWarehouseId);

                        if (toItemWarehouse != null)
                        {
                            toItemWarehouse.StockQuantity += item.TransferQuantity;
                            await _inventoryUnitOfWork.ItemWarehouseRepository
                                .EditAsync(toItemWarehouse);
                        }
                        else
                        {
                            var newItemWarehouse = new ItemWarehouse
                            {
                                ItemId = item.ItemId,
                                WarehouseId = toWarehouseId,
                                StockQuantity = item.TransferQuantity,
                                CostPerUnit = itemWarehouse?.CostPerUnit,
                                AsOfDate = transferDate,
                            };

                            await _inventoryUnitOfWork.ItemWarehouseRepository
                                .AddAsync(newItemWarehouse);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Transfer Quantity can not be bigger than Available Stock");
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Item warehouse not found");
                }
            }
        }
    }
}
