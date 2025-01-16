using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class ItemWarehouse : ICompositEntity<Guid>
    {
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public double? StockQuantity { get; set; }
        public decimal? CostPerUnit { get; set; }
        public DateTime? AsOfDate { get; set; }
    }
}
