using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Inventory.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<ItemCategory, Guid>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IList<ItemCategory> data, int total, int totalDisplay)> GetPagedCategoriesAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            var searchText = search.Value;
            if (string.IsNullOrWhiteSpace(searchText))
                return await GetDynamicAsync(null, order, null, pageIndex, pageSize, true);

            return await GetDynamicAsync(x => x.Name.Contains(searchText) ||
                                         x.Description.Contains(searchText),
                                         order, null, pageIndex, pageSize, true);
        }

        public bool IsTitleDuplicate(string title, Guid? id = null)
        {
            if (id.HasValue)
            {
                return GetCount(x => !x.Id.Equals(id.Value) && x.Name.Equals(title)) > 0;
            }
            else
            {
                return GetCount(x => x.Name.Equals(title)) > 0;
            }
        }

        public async Task<IList<ItemCategory>> GetOrderedItemCategoriesAsync()
        {
            return await GetAsync(null, x => x.OrderBy(y => y.Name), null, true);
        }
    }
}
