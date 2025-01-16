using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public class ItemManagementService : IItemManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public ItemManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public async Task CreateItemAsync(Item item)
        {
            var isTitleDuplicate = _inventoryUnitOfWork.ItemRepository.IsTitleDuplicate(item.ItemName);

            if (!isTitleDuplicate)
            {
                await _inventoryUnitOfWork.ItemRepository.AddAsync(item);
                await _inventoryUnitOfWork.SaveAsync();
            }
            else
            {
                throw new Exception("Name is duplicate");
            }
        }

        public async Task CreateItemWarehouseAsync(List<ItemWarehouse> itemWarehouses)
        {
            await _inventoryUnitOfWork.ItemWarehouseRepository.AddAsync(itemWarehouses);
            await _inventoryUnitOfWork.SaveAsync();
        }

        public async Task DeleteItemAsync(Guid id)
        {
            await _inventoryUnitOfWork.ItemRepository.RemoveAsync(id);
            await _inventoryUnitOfWork.SaveAsync();
        }

        public async Task DeleteMultipleItemsAsync(List<Guid>? selectedItemIds)
        {
            if (selectedItemIds != null && selectedItemIds.Count > 0)
            {
                await _inventoryUnitOfWork.ItemRepository.RemoveMultipleAsync(selectedItemIds);
                await _inventoryUnitOfWork.SaveAsync();
            }
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            return await _inventoryUnitOfWork.ItemRepository.GetItemByIdAsync(id);
        }

        public async Task<(IList<ItemListDto> data, int total, int totalDisplay)> GetItemsAsync(int pageIndex, int pageSize, ItemSearchDto search, string? order)
        {
            return await _inventoryUnitOfWork.ItemRepository.GetPagedItemListUsingSPAsync(pageIndex, pageSize, search, order);
        }

        public async Task UpdateItemAsync(Item item)
        {
            var isTitleDuplicate = _inventoryUnitOfWork.ItemRepository.IsTitleDuplicate(item.ItemName, item.Id);

            if (!isTitleDuplicate)
            {
                await _inventoryUnitOfWork.ItemRepository.EditAsync(item);
                await _inventoryUnitOfWork.SaveAsync();
            }
            else
            {
                throw new Exception("Name is duplicate");
            }
        }

        public async Task<IList<Item>> GetItemsForStockTransferAsync(string searchItem, Guid warehouseId)
        {
            return await _inventoryUnitOfWork.ItemRepository.GetFilteredItemsAsync(searchItem, warehouseId);
        }

        public async Task<double> GetItemsCountAsync()
        {
            return await _inventoryUnitOfWork.ItemRepository.GetCountAsync();
        }
    }
}
