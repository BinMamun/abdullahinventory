using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public interface ITaxManagementService
    {
        Task<(IList<Tax> data, int total, int totalDisplay)> GetTaxesAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order);

        Task<Tax> GetTaxAsync(Guid taxId);
        Task<IList<Tax>> GetTaxListAsync();
    }
}
