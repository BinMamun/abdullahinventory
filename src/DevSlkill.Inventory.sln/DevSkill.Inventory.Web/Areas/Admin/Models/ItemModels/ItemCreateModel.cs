using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.ItemModels
{
    public class ItemCreateModel : ItemCommonModel
    {
        public DateTime? AsOfDate { get; set; }
        public decimal? CostPerUnit { get; set; }
        public void SetItemWahouses(IList<Warehouse> warehouses)
        {
            Warehouses = (from w in warehouses
                          select new ItemWarehouseModel() {
                              WarehouseId = w.Id,
                              WarehouseName = w.Name }).ToList();

        } 
    }
}
