using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public interface IServiceManagementService
    {
        Task CreateServiceAsync(Service service);
        Task<(IList<Service> data, int total, int totalDisplay)> GetServicesAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order);

        Task DeleteServiceAsync(Guid id);
        Task<Service> GetServiceAsync(Guid id);
        Task UpdateServiceAsync(Service service);
        Task<double> GetServicesCountAsync();
    }
}
