using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Inventory.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class StockTransferRepository : Repository<StockTransfer, Guid>, IStockTransferRepository
    {
        public StockTransferRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IList<StockTransfer> data, int total, int totalDisplay)> GetPagedStockTransfersAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return await GetDynamicAsync(null, order, 
                x => x.Include(y => y.FromWarehouse)
                      .Include(z => z.ToWarehouse), pageIndex, pageSize, true);
        }

        public async Task<StockTransfer> GetStockTransferWithItemsAsync(Guid id)
        {
            return await GetByIdAsync(x => x.Id.Equals(id), 
                                            x => x.Include(
                                                    y => y.StockTransferItems)
                                                            .ThenInclude(i => i.Item)
                                                            .ThenInclude(m => m.MeasurementUnit));
        }
    }
}
