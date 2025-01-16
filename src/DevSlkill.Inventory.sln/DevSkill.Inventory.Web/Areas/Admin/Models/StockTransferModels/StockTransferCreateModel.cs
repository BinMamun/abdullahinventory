using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.StockTransferModels
{
    public class StockTransferCreateModel
    {
        [Display(Name ="Warehouse From")]
        public Guid FromWarehouseId { get; set; }
        [Display(Name = "Warehouse To")]
        public Guid ToWarehouseId { get; set; }
        public DateTime TransferDate { get; set; }
        public string? Note { get; set; }
        public IList<StockTransferItemModel> StockTransferItems { get; set; }
        public IList<SelectListItem>? Warehouses { get; private set; }

        public void SetWarhouseValues(IList<Warehouse> warehouses)
        {
            Warehouses = warehouses.ToSelectList(x => x.Name, y => y.Id);
        }
    }
}
