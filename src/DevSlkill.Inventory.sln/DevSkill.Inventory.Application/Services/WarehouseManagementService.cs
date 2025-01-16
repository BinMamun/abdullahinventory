using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public class WarehouseManagementService : IWarehouseManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public WarehouseManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public async Task CreateWarehouseAsync(Warehouse warehouse)
        {
            var isTitleDuplicate = _inventoryUnitOfWork.WarehouseRepository.IsTitleDuplicate(warehouse.Name);

            if (!isTitleDuplicate)
            { 
                await _inventoryUnitOfWork.WarehouseRepository.AddAsync(warehouse);
                await _inventoryUnitOfWork.SaveAsync();
            }
            else
            {
                throw new Exception("Warehouse name is duplicate");
            }
        }


        public async Task UpdateWarehouseAsync(Warehouse warehouse)
        {
            var isTitleDuplicate = _inventoryUnitOfWork.WarehouseRepository.IsTitleDuplicate(warehouse.Name, warehouse.Id);
            if (!isTitleDuplicate)
            {
                await _inventoryUnitOfWork.WarehouseRepository.EditAsync(warehouse);
                await _inventoryUnitOfWork.SaveAsync();
            }
            else
            {
                throw new Exception("Warehouse name is duplicate");
            }
        }
        public async Task<Warehouse> GetWarehouseAsync(Guid id)
        {
            return await _inventoryUnitOfWork.WarehouseRepository.GetByIdAsync(id);
        }

        public async Task<IList<Warehouse>> GetWarehouseListAsync()
        {
            return await _inventoryUnitOfWork
                        .WarehouseRepository
                        .GetWarehousesWithItemWarehouseAsync();
        }

        public async Task<(IList<Warehouse> data, int total, int totalDisplay)> GetWarehousesAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return await _inventoryUnitOfWork.WarehouseRepository.GetPagedWarehousesAsync(pageIndex, pageSize, search, order);
        }

        public async Task<IList<Warehouse>> GetOrderedWarehousesAsync()
        {
            return await _inventoryUnitOfWork.WarehouseRepository.GetOrderedWarehouseListAsync();
        }

        public async Task DeleteWarehouseAsync(Guid id)
        {
            await _inventoryUnitOfWork.WarehouseRepository.RemoveAsync(id);
            await _inventoryUnitOfWork.SaveAsync();
        }
    }
}
