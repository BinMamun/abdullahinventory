using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.StockAdjustmentModels
{
    public class StockAdjustmentCreateModel
    {
        public DateTime AdjustDate {  get; set; }
        public string? Note {  get; set; }
        public Guid StockAdjustmentReasonId { get; set; }
        public Guid WarehouseId { get; set; }
        public IList<StockAdjustmentItemModel> StockAdjustmentItems { get; set; }
        public IList<SelectListItem>? Reasons { get; private set; }
        public IList<SelectListItem>? Warehouses{ get; private set; }
        public StockAdjustmentReasonCreateModel? ReasonName { get; set; }

        public void SetReasons(IList<StockAdjustmentReason> reasons)
        {
            Reasons = reasons.ToSelectList(x => x.Name, y => y.Id);
        }

        public void SetWarhouses(IList<Warehouse> warehouses)
        {
            Warehouses = warehouses.ToSelectList(x => x.Name, y => y.Id);
        }
    }
}
