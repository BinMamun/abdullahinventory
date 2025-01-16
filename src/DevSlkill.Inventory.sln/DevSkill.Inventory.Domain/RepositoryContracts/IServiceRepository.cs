using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface IServiceRepository : IRepositoryBase<Service, Guid>
    {
        bool IsTitleDuplicate(string title, Guid? id = null);

        Task<(IList<Service> data, int total, int totalDisplay)> GetServiceList(int pageIndex, int pageSize, DataTablesSearch search, string? order);
        Task<Service> GetServiceAsync(Guid serviceId);
    }
}
