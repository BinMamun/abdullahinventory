using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public interface ICategoryManagementService
    {
        Task CreateItemCategoryAsync(ItemCategory category);
        Task DeleteCategoryAsync(Guid id);
        Task<(IList<ItemCategory> data, int total, int totalDisplay)> GetCategoriesAsync(
            int pageIndex, int pageSize, DataTablesSearch search, string? order);
        Task<ItemCategory> GetCategoryAsync(Guid id);
        Task UpdateCategoryAsync(ItemCategory category);

        Task<IList<ItemCategory>> GetCategoriesAsync();
    }
}
