using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Inventory.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class ServiceRepository : Repository<Service, Guid>, IServiceRepository
    {
        public ServiceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IList<Service> data, int total, int totalDisplay)> GetServiceList(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return await GetDynamicAsync(null, order, x => x.Include(y => y.Tax), pageIndex, pageSize, true);
        }

        public async Task<Service> GetServiceAsync(Guid serviceId)
        {
            return await GetByIdAsync(x => x.Id.Equals(serviceId), y => y.Include(z => z.Tax));
        }

        public bool IsTitleDuplicate(string title, Guid? id = null)
        {
            if (id.HasValue)
            {
                return GetCount(x => !x.Id.Equals(id.Value) && x.ServiceName.Equals(title)) > 0;
            }
            else
            {
                return GetCount(x => x.ServiceName.Equals(title)) > 0;
            }
        }
    }
}
