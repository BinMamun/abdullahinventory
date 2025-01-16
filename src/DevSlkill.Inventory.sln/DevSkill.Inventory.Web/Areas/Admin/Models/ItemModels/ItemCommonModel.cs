using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.ItemModels
{
    public class ItemCommonModel
    {
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Buying Price")]
        public decimal BuyingPrice { get; set; }

        [Display(Name = "Selling Price")]
        public decimal SellingPrice { get; set; }
        public string? Barcode { get; set; }
        [Display(Name = "Is Active Item?")]
        public bool IsActive { get; set; }
        [Display(Name = "Minimum Stock Quantity")]
        public int? MinimumStockQuantity { get; set; }
        public string? Description { get; set; }
        public IFormFile? Picture { get; set; }

        [Display(Name = "Tax Category")]
        public Guid? TaxId { get; set; }
        public IList<SelectListItem>? Taxes { get; private set; }

        [Display(Name = "Categories")]
        public Guid? CategoryId { get; set; }
        public IList<SelectListItem>? Categories { get; private set; }

        [Display(Name = "Measurement Unit")]
        public Guid MeasurementUnitId { get; set; }
        public IList<SelectListItem>? MeasurementUnits { get; private set; }

        public List<ItemWarehouseModel>? Warehouses { get; set; }

        public void SetTaxValues(IList<Tax> taxes)
        {
            Taxes = taxes.ToSelectList(x => x.Name, y => y.Id);
        }

        public void SetCategories(IList<ItemCategory> categories)
        {
            Categories = categories.ToSelectList(x => x.Name, y => y.Id);
        }
        public void SetMeasurementUnits(IList<MeasurementUnit> measurementUnits)
        {
            MeasurementUnits = measurementUnits.ToSelectList(x => x.Name, y => y.Id);
        }
    }
}
