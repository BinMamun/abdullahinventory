using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface IMeasurementUnitRepository : IRepositoryBase<MeasurementUnit, Guid>
    {
        bool IsTitleDuplicate(string title, Guid? id = null);
        Task<(IList<MeasurementUnit> data, int total, int totalDisplay)> GetPagedMeasurementUnitsAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order);
    }
}
