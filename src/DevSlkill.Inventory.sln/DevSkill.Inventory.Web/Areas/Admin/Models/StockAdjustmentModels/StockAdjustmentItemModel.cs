namespace DevSkill.Inventory.Web.Areas.Admin.Models.StockAdjustmentModels
{
    public class StockAdjustmentItemModel
    {
        public Guid ItemId { get; set; }
        public double AdjustedQuantity { get; set; }
        public bool IsIncrease { get; set; }
    }
}
