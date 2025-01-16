using DevSkill.Inventory.Web.Areas.Admin.Models.LogModels;
using DevSkill.Inventory.Web.Areas.Admin.Models.ServiceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    public class LogController : Controller
    {
        [Area("Admin"), Authorize(Roles ="Super Admin")]
        public IActionResult Index()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<JsonResult> GetLogsJsonData([FromBody] LogListModel model)
        //{
        //    var result = await _serviceManagementService.GetServicesAsync(
        //        model.PageIndex,
        //        model.PageSize,
        //        model.Search,
        //        model.FormatSortExpression("ServiceName", "Tax", "SellingPrice"));

        //    var serviceJsonData = new
        //    {
        //        recordsTotal = result.total,
        //        recordsFiltered = result.totalDisplay,
        //        data = (from records in result.data
        //                select new string[]
        //                    {
        //                        HttpUtility.HtmlEncode(records.ServiceName),
        //                        HttpUtility.HtmlEncode(records.Tax?.Parcentage.ToString()),
        //                        HttpUtility.HtmlEncode(records.SellingPrice),
        //                        records.Id.ToString()
        //                    }
        //                ).ToArray()
        //    };
        //    return Json(serviceJsonData);
        //}
    }
}
