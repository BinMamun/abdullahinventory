using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class StockTransfer : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid FromWarehouseId { get; set; }
        public Warehouse FromWarehouse { get; set; }
        public Guid ToWarehouseId { get; set; }
        public Warehouse ToWarehouse { get; set; }
        public DateTime TransferDate { get; set; }
        public string? Note { get; set; }
        public List<StockTransferItem> StockTransferItems { get; set; }
        public string UserName { get; set; }
    }
}
