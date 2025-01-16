using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class StockTransferItem : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public double TransferQuantity { get; set; }
        public StockTransfer StockTransfer { get; set; }
        public Item Item { get; set; }
    }
}
