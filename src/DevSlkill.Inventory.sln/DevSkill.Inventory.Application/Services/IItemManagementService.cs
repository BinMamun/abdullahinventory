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
    public interface IItemManagementService
    {
        Task CreateItemAsync(Item item);
        Task<Item> GetItemAsync(Guid id);
        Task DeleteItemAsync(Guid id);
        Task<(IList<ItemListDto> data, int total, int totalDisplay)> GetItemsAsync(int pageIndex, int pageSize, ItemSearchDto search, string? order);
        Task UpdateItemAsync(Item item);

        Task CreateItemWarehouseAsync(List<ItemWarehouse> warehouses);
        Task DeleteMultipleItemsAsync(List<Guid>? selectedItemIds);
        Task<IList<Item>> GetItemsForStockTransferAsync(string searchItem, Guid warehouseId);
        Task<double> GetItemsCountAsync();
    }
}
