using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class StockAdjustmentItem : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public double AdjustedQuantity { get; set; }
        public bool IsIncrease { get; set; }
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        public StockAdjustment StockAdjustment { get; set; }
    }
}
