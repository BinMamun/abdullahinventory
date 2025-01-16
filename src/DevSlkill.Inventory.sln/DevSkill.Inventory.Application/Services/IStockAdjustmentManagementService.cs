using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public interface IStockAdjustmentManagementService
    {
        Task CreateStockAdjustmentAsync(StockAdjustment stockAdjustment, List<StockAdjustmentItem> stockAdjustmentItems);
        Task CreateStockAdjustmentReasonAsync(StockAdjustmentReason reason);
        Task DeleteStockAdjustmentAsync(Guid id);
        Task<IList<StockAdjustmentReason>> GetStockAdjustmentReasonsAsync();
        Task<(IList<StockAdjustment> data, int total, int totalDisplay)> GetStockAdjustmentsAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order);
    }
}
