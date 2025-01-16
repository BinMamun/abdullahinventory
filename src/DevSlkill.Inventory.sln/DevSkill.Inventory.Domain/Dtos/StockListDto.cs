using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class StockListDto
    {
        public string ItemName { get; set; }
        public string? Barcode { get; set; }
        public string? WarehouseName { get; set; }
        public string? CategoryName { get; set; }
        public double StockQuantity { get; set; }
        public double MinimumStockQuantity { get; set; }
        public string Symbol { get; set; }
        public double CostPerUnit { get; set; }
    }
}
