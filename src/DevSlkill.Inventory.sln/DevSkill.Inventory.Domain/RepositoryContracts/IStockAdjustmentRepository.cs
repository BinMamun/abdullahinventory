using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface IStockAdjustmentRepository : IRepositoryBase<StockAdjustment, Guid>
    {
        Task<(IList<StockAdjustment> data, int total, int totalDisplay)> GetPagedStockAdjustmentListAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order);
        Task<StockAdjustment> GetStockAdjustmentByIdWithItemsAsync(Guid id);
    }
}
