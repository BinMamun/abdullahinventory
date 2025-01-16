using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface ICategoryRepository : IRepositoryBase<ItemCategory, Guid>
    {
        Task<(IList<ItemCategory> data, int total, int totalDisplay)> GetPagedCategoriesAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order);
        bool IsTitleDuplicate(string title, Guid? id = null);
        Task<IList<ItemCategory>> GetOrderedItemCategoriesAsync();
    }
}
