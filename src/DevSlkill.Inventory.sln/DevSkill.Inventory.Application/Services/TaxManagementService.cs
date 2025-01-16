using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public class TaxManagementService : ITaxManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public TaxManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public async Task<(IList<Tax> data, int total, int totalDisplay)> GetTaxesAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return await _inventoryUnitOfWork.TaxRepository.GetAllTaxesAsync(pageIndex, pageSize, search, order);
        }

        public async Task<Tax> GetTaxAsync(Guid taxId)
        {
            return await _inventoryUnitOfWork.TaxRepository.GetByIdAsync(taxId);
        }

        public async Task<IList<Tax>> GetTaxListAsync()
        {
            return await _inventoryUnitOfWork.TaxRepository.GetOrderedTaxes();
        }
    }
}
