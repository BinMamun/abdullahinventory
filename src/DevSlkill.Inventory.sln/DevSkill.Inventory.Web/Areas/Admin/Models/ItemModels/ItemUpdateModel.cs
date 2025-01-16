using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.ItemModels
{
    public class ItemUpdateModel : ItemCommonModel
    {
        public Guid Id { get; set; }
        public string? PicturePath { get; set; }

        public void GetItemWahouses(IList<Warehouse> warehouses)
        {
            Warehouses = (from w in warehouses
                          select new ItemWarehouseModel()
                          {
                              WarehouseId = w.Id,
                              WarehouseName = w.Name,
                              StockQuantity = w.ItemWarehouses?
                                                .Where(x => x.ItemId.Equals(Id))
                                                .Select(x => x.StockQuantity) 
                                                .FirstOrDefault()

                          }).ToList();

        }
    }
}
