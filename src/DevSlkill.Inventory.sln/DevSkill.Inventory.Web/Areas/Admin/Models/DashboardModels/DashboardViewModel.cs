using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.DashboardModels
{
    public class DashboardViewModel
    {
        public double TotalItemCount {  get; set; }
        public double TotalServiceCount {  get; set; }
        public decimal? TotalStockValue { get; set; }

        public async Task<decimal?> GetStockValue(IList<ItemWarehouse> itemWarehouses)
        {
            return itemWarehouses.Sum(x => (decimal)x.StockQuantity * x.CostPerUnit);
        }
    }
}
