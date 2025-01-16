using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class Item : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; }
        public decimal BuyingPrice {  get; set; }
        public decimal SellingPrice { get; set; }
        public string? Barcode { get; set; }
        public string? Picture { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int? MinimumStockQuantity { get; set; }
        public Guid? TaxId { get; set; }
        public Tax? Tax { get; set; }
        public Guid? CategoryId { get; set; }
        public ItemCategory? Category { get; set; }
        public Guid MeasurementUnitId { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }

        public List<ItemWarehouse>? ItemWarehouses { get; set; }
    }
}
