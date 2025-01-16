using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure.Utilities;
using Inventory.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.StockListModels
{
    public class StockListDtoModel : DataTables
    {
        public StockListSearchDto SearchItem { get; set; }
        public IList<SelectListItem>? Categories { get; private set; }
        public IList<SelectListItem>? Warehouses { get; private set; }

        public void SetCategories(IList<ItemCategory> categories)
        {
            Categories = categories.ToSelectList(x => x.Name, y => y.Id);
        }

        public void SetWarehouses(IList<Warehouse> warehouses)
        {
            Warehouses = warehouses.ToSelectList(x => x.Name, y => y.Id);
        }
    }
}
