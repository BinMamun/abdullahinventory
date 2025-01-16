using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface ITaxRepository : IRepositoryBase<Tax, Guid>
    {
        Task<(IList<Tax> data, int total, int totalDisplay)> GetAllTaxesAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order);

        Task<IList<Tax>> GetOrderedTaxes();
    }
}
