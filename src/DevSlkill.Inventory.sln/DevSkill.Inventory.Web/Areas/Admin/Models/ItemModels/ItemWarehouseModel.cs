namespace DevSkill.Inventory.Web.Areas.Admin.Models.ItemModels
{
    public class ItemWarehouseModel
    {
        public Guid WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
        public double? StockQuantity { get; set; }
    }
}
