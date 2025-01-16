using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface IStockTransferRepository : IRepositoryBase<StockTransfer, Guid>
    {
        Task<(IList<StockTransfer> data, int total, int totalDisplay)> GetPagedStockTransfersAsync
            (int pageIndex, int pageSize, DataTablesSearch search, string? order);
        Task<StockTransfer> GetStockTransferWithItemsAsync(Guid id);
    }
}
