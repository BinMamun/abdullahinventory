using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using DevSkill.Inventory.Infrastructure.UnitOfWorks;
using DevSkill.Inventory.Infrastructure.Utilities;
using Inventory.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class ItemRepository : Repository<Item, Guid>, IItemRepository
    {
        public ItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public bool IsTitleDuplicate(string title, Guid? id = null)
        {
            if (id.HasValue)
            {
                return GetCount(x => !x.Id.Equals(id.Value) && x.ItemName.Equals(title)) > 0;
            }
            else
            {
                return GetCount(x => x.ItemName.Equals(title)) > 0;
            }
        }

        public async Task<(IList<Item> data, int total, int totalDisplay)> GetPagedItemListAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            var searchText = search.Value;
            if (string.IsNullOrWhiteSpace(searchText))
                return await GetDynamicAsync(null, order,
                                        x => x
                                        .Include(t => t.Tax)
                                        .Include(c => c.Category)
                                        .Include(m => m.MeasurementUnit)
                                        .Include(iw => iw.ItemWarehouses),
                                        pageIndex, pageSize, true);

            return await GetDynamicAsync(x => x.ItemName.Contains(searchText)
                                         , order,
                                         x => x
                                         .Include(t => t.Tax)
                                         .Include(c => c.Category)
                                         .Include(m => m.MeasurementUnit)
                                         .Include(iw => iw.ItemWarehouses),
                                         pageIndex, pageSize, true);
        }

        public async Task<(IList<ItemListDto> data, int total, int totalDisplay)> GetPagedItemListUsingSPAsync(
            int pageIndex, int pageSize, ItemSearchDto search, string? order)
        {
            var procedureName = "GetItemList";

            var result = await SqlUtility.QueryWithStoredProcedureAsync<ItemListDto>(
                procedureName,
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
                    { "PriceFrom", search.PriceFrom.HasValue ? search.PriceFrom : null},
                    { "PriceTo", search.PriceTo.HasValue ? search.PriceTo : null},
                    { "IsActive", search.IsActive == null ? null : search.IsActive },
                    { "BelowMinimumStock", search.BelowMinimumStock }
                },
                new Dictionary<string, Type>
                {
                    { "Total", typeof(int) },
                    { "TotalDisplay", typeof(int) }
                });

            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }

        public async Task<Item> GetItemByIdAsync(Guid id)
        {
            return await GetByIdAsync(x => x.Id.Equals(id), 
                                             y =>
                                             y.Include(t => t.Tax)
                                             .Include(c => c.Category)
                                             .Include(m => m.MeasurementUnit));
        }

        public async Task RemoveMultipleAsync(List<Guid> selectedItemIds)
        {
            var entities = await GetByIdsAsync<Item>(selectedItemIds);
            await RemoveRangeAsync(entities);
        }

        public async Task<IList<Item>> GetFilteredItemsAsync(string searchItem, Guid warehouseId)
        {
            return await GetAsync(x => x.ItemName.Contains(searchItem), null,
                                    x => x.Include(y => y.ItemWarehouses
                                                       .Where(z => z.WarehouseId
                                                        .Equals(warehouseId))), true);
        }
    }
}
