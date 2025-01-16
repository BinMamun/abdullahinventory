using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Web.Areas.Admin.Models.CategoryModels;
using DevSkill.Inventory.Web.Areas.Admin.Models.TaxModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Super Admin, Admin")]
    public class TaxController : Controller
    {
        private readonly ILogger<TaxController> _logger;
        private readonly ITaxManagementService _taxManagementService;

        public TaxController(
            ILogger<TaxController> logger,
            ITaxManagementService taxManagementService)
        {
            _logger = logger;
            _taxManagementService = taxManagementService;
        }

        [Authorize(Policy = "ViewIndexPolicy")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetTaxJsonData([FromBody] TaxListModel model)
        {
            var result = await _taxManagementService.GetTaxesAsync(
                model.PageIndex,
                model.PageSize,
                model.Search,
                model.FormatSortExpression("Name", "Parcentage"));

            var taxJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from records in result.data
                        select new string[]
                            {
                                HttpUtility.HtmlEncode(records.Name),
                                HttpUtility.HtmlEncode(records.Parcentage),
                            }
                        ).ToArray()
            };
            return Json(taxJsonData);
        }
    }
}
