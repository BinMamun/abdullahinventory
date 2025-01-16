using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using DevSkill.Inventory.Infrastructure.UnitOfWorks;
using Inventory.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class MeasurementUnitRepository : Repository<MeasurementUnit, Guid>, IMeasurementUnitRepository
    {
        public MeasurementUnitRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public bool IsTitleDuplicate(string title, Guid? id = null)
        {
            if (id.HasValue)
            {
                return GetCount(x => !x.Id.Equals(id.Value) && x.Name.Equals(title)) > 0;
            }
            else
            {
                return GetCount(x => x.Name.Equals(title)) > 0;
            }
        }

        public async Task<(IList<MeasurementUnit> data, int total, int totalDisplay)> GetPagedMeasurementUnitsAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            var searchText = search.Value;
            if (string.IsNullOrWhiteSpace(searchText))
                return await GetDynamicAsync(null, order, null, pageIndex, pageSize, true);

            return await GetDynamicAsync(x => x.Name.Contains(searchText) ||
                                         x.Symbol.Contains(searchText),
                                         order, null, pageIndex, pageSize, true);
        }
    }
}
