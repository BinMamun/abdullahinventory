using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.StockAdjustmentModels
{
    public class StockAdjustmentReasonCreateModel
    {
        [Required]
        public string ReasonName { get; set; }
    }
}
