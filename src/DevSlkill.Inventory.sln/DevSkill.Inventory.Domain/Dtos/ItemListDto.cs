using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class ItemListDto
    {
        public Guid Id { get; set; }
        public string? Picture { get; set; }
        public string ItemName { get; set; }
        public string? Barcode { get; set; }
        public string? Category { get; set; }
        public double Price { get; set; }
        public double? Tax { get; set; }
        public int? MinimumStockQuantity { get; set; }
        public double? TotalStockQuantity { get; set; }
        public string? MeasurementUnit { get; set; }
        public bool IsActive { get; set; }
    }
}
