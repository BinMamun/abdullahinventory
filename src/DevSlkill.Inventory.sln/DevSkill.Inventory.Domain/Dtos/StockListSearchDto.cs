using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class StockListSearchDto
    {
        public string? ItemName { get; set; }
        public string? Barcode { get; set; }
        public string? CategoryId { get; set; }
        public string? WarehouseId { get; set; }
        public bool StockGreaterThanZero { get; set; }
        public bool BelowMinimumStock { get; set; }
    }
}
