using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public interface IStockTransferManagementService
    {
        Task CreateStockTransferAsync(StockTransfer stockTransfer, List<StockTransferItem> stockTransferItems);
        Task DeleteStockTransferAsync(Guid id);
        Task<StockTransfer> GetStockTransferItemsAsync(Guid id);
        Task<(IList<StockTransfer> data, int total, int totalDisplay)> GetStockTransfersAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order);
    }
}
