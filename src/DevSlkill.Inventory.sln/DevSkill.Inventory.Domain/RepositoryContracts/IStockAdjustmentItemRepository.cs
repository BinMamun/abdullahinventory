using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface IStockAdjustmentItemRepository : IRepositoryBase<StockAdjustmentItem, Guid>
    {
    }
}
