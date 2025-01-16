using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class StockAdjustment : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public DateTime AdjustDate { get; set; }
        public Guid WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }
        public Guid StockAdjustmentReasonId { get; set; }
        public StockAdjustmentReason? StockAdjustmentReason { get; set; }
        public string? Note { get; set; }
        public List<StockAdjustmentItem>? StockAdjustmentItems { get; set; }
        public string UserName { get; set; }
    }
}
