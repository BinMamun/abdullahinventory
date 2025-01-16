using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public class CategoryManagementService : ICategoryManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public CategoryManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public async Task<(IList<ItemCategory> data, int total, int totalDisplay)> GetCategoriesAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return await _inventoryUnitOfWork.CategoryRepository.GetPagedCategoriesAsync(
                pageIndex, pageSize, search, order);
        }

        public async Task CreateItemCategoryAsync(ItemCategory category)
        {
            var isDuplicateTitle = _inventoryUnitOfWork.CategoryRepository.IsTitleDuplicate(category.Name);

            if (!isDuplicateTitle)
            {
                await _inventoryUnitOfWork.CategoryRepository.AddAsync(category);
                await _inventoryUnitOfWork.SaveAsync();
            }
            else
            {
                throw new Exception("Category name is duplicate");
            }
        }

        public async Task<ItemCategory> GetCategoryAsync(Guid id)
        {
            return await _inventoryUnitOfWork.CategoryRepository.GetByIdAsync(id);
        }

        public async Task UpdateCategoryAsync(ItemCategory category)
        {
            if (!_inventoryUnitOfWork.CategoryRepository.IsTitleDuplicate(category.Name, category.Id))
            {
                await _inventoryUnitOfWork.CategoryRepository.EditAsync(category);
                await _inventoryUnitOfWork.SaveAsync();
            }
            else
            {
                throw new Exception("Category name is duplicate");
            }
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            await _inventoryUnitOfWork.CategoryRepository.RemoveAsync(id);
            await _inventoryUnitOfWork.SaveAsync();
        }

        public async Task<IList<ItemCategory>> GetCategoriesAsync()
        {
            return await _inventoryUnitOfWork.CategoryRepository.GetOrderedItemCategoriesAsync();
        }
    }
}
