using AutoMapper;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Web.Areas.Admin.Models.CategoryModels;
using DevSkill.Inventory.Web.Areas.Admin.Models.StockListModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class StockListController : Controller
    {
        private readonly ILogger<StockListController> _logger;
        private readonly IMapper _mapper;
        private readonly IDisplayStockListService _displayStockListService;
        private readonly ICategoryManagementService _categoryManagementService;
        private readonly IWarehouseManagementService _warehouseManagementService;

        public StockListController(
            ILogger<StockListController> logger,
            IMapper mapper,
            IDisplayStockListService displayStockListService,
            ICategoryManagementService categoryManagementService,
            IWarehouseManagementService warehouseManagementService
            )
        {
            _logger = logger;
            _mapper = mapper;
            _displayStockListService = displayStockListService;
            _categoryManagementService = categoryManagementService;
            _warehouseManagementService = warehouseManagementService;
        }

        [Authorize(Policy = "ViewIndexPolicy")]
        public async Task<IActionResult> Index()
        {
            var model = new StockListDtoModel();

            var categories = await _categoryManagementService.GetCategoriesAsync();
            var warehouses = await _warehouseManagementService.GetOrderedWarehousesAsync();

            model.SetCategories(categories);
            model.SetWarehouses(warehouses);

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> GetStockListJsonData([FromBody] StockListDtoModel model)
        {
            var result = await _displayStockListService.GetStockListAsync(
                model.PageIndex,
                model.PageSize,
                model.SearchItem,
                model.FormatSortExpression("ItemName", "Barcode", "CategoryName", "WarehouseName",
                "MinimumStockQuantity", "StockQuantity", "CostPerUnit"));

            var stockListJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from records in result.data
                        select new string[]
                            {
                                HttpUtility.HtmlEncode(records.ItemName),
                                HttpUtility.HtmlEncode(records.Barcode),
                                HttpUtility.HtmlEncode(records.CategoryName),
                                HttpUtility.HtmlEncode(records.WarehouseName),
                                HttpUtility.HtmlEncode(records.MinimumStockQuantity),
                                HttpUtility.HtmlEncode($"{records.StockQuantity} {records.Symbol}"),
                                HttpUtility.HtmlEncode(records.CostPerUnit)
                            }
                        ).ToArray()
            };
            return Json(stockListJsonData);
        }
    }
}
