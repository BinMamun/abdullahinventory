using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public interface IWarehouseManagementService
    {
        Task CreateWarehouseAsync(Warehouse warehouse);
        Task UpdateWarehouseAsync(Warehouse warehouse);

        Task<Warehouse> GetWarehouseAsync(Guid id);
        Task<IList<Warehouse>> GetWarehouseListAsync();
        Task<(IList<Warehouse> data, int total, int totalDisplay)> GetWarehousesAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order);
        Task DeleteWarehouseAsync(Guid id);
        Task<IList<Warehouse>> GetOrderedWarehousesAsync();
    }
}
