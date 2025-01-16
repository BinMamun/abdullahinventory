using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.ServiceModels
{
    public class ServiceCommonModel
    {
        [Display(Name = "Service Name")]
        public string ServiceName { get; set; }

        [Display(Name = "Buying Price")]
        public double? BuyingPrice { get; set; }

        [Display(Name = "Selling Price")]
        public double? SellingPrice { get; set; }
        public string? Description { get; set; }

        [Display(Name = "Tax Category")]
        public Guid? TaxId { get; set; }
        public IList<SelectListItem>? Taxes { get; private set; }

        public void SetTaxValues(IList<Tax> taxes)
        {
            Taxes = taxes.ToSelectList(x => x.Name, y => y.Id);
        }
    }
}
