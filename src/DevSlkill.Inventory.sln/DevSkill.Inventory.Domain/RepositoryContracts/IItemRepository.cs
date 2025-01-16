using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface IItemRepository : IRepositoryBase<Item, Guid>
    {
        bool IsTitleDuplicate(string title, Guid? id = null);
        Task<(IList<Item> data, int total, int totalDisplay)> GetPagedItemListAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order);
        Task<(IList<ItemListDto> data, int total, int totalDisplay)> GetPagedItemListUsingSPAsync(
         int pageIndex, int pageSize, ItemSearchDto search, string? order);
        Task<Item> GetItemByIdAsync(Guid id);
        Task RemoveMultipleAsync(List<Guid> selectedItemIds);
        Task<IList<Item>> GetFilteredItemsAsync(string searchItem, Guid warehouseId);
    }
}
