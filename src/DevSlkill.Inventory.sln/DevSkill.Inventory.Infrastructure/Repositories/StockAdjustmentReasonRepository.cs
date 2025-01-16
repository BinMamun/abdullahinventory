using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class StockAdjustmentReasonRepository : Repository<StockAdjustmentReason, Guid>, IStockAdjustmentReasonRepository
    {
        public StockAdjustmentReasonRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
