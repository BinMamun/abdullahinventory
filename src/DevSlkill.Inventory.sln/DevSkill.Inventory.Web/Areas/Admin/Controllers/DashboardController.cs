using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Web.Areas.Admin.Models.DashboardModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IItemManagementService _itemManagementService;
        private readonly IServiceManagementService _serviceManagementService;
        private readonly IDisplayStockListService _stockListService;

        public DashboardController(
            ILogger<DashboardController> logger,
            IItemManagementService itemManagementService,
            IServiceManagementService serviceManagementService,
            IDisplayStockListService stockListService)
        {
            _logger = logger;
            _itemManagementService = itemManagementService;
            _serviceManagementService = serviceManagementService;
            _stockListService = stockListService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel();
            var totalStockedItems = await _stockListService.GetTotalStockValueAsync();

            model.TotalItemCount = await _itemManagementService.GetItemsCountAsync();
            model.TotalServiceCount = await _serviceManagementService.GetServicesCountAsync();
            model.TotalStockValue = await model.GetStockValue(totalStockedItems);
            
            return View(model);
        }
    }
}
