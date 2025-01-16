using AutoMapper;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure.Extensions;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Admin.Models.StockAdjustmentModels;
using DevSkill.Inventory.Web.Areas.Admin.Models.StockTransferModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class StockAdjustmentController : Controller
    {
        private readonly ILogger<StockAdjustmentController> _logger;
        private readonly IMapper _mapper;
        private readonly IStockAdjustmentManagementService _stockAdjustmentManagementService;
        private readonly IWarehouseManagementService _warehouseManagementService;
        private readonly IItemManagementService _itemManagementService;
        public StockAdjustmentController(
            ILogger<StockAdjustmentController> logger,
            IMapper mapper,
            IStockAdjustmentManagementService stockAdjustmentManagementService,
            IWarehouseManagementService warehouseManagementService,
            IItemManagementService itemManagementService
            )
        {
            _logger = logger;
            _mapper = mapper;
            _stockAdjustmentManagementService = stockAdjustmentManagementService;
            _warehouseManagementService = warehouseManagementService;
            _itemManagementService = itemManagementService;
        }

        [Authorize(Policy = "ViewIndexPolicy")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetStockAdjustmentJsonData([FromBody] StockAdjustmentListModel model)
        {
            var result = await _stockAdjustmentManagementService.GetStockAdjustmentsAsync(
                model.PageIndex,
                model.PageSize,
                model.Search,
                model.FormatSortExpression("AdjustDate", "WarehouseId", "Warehouse.Name", "StockAdjustmentReasonId", "StockAdjustmentReason", "Note", "Id"));

            var stockAdjustmentJsonData = new
            {

                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from records in result.data
                        select new string[]
                            {
                                HttpUtility.HtmlEncode(records.AdjustDate.ToString("dd-MM-yyyy")),
                                HttpUtility.HtmlEncode(string
                                .Join("<br/>", records.StockAdjustmentItems
                                        .Select(item => item.Item.ItemName)))
                                        .Replace("&lt;br/&gt;", "<br/>"),
                                HttpUtility.HtmlEncode(records.Warehouse.Name),
                                HttpUtility.HtmlEncode(records.StockAdjustmentItems
                                .Sum(x => x.AdjustedQuantity).ToString()),
                                HttpUtility.HtmlEncode(records.StockAdjustmentReason.Name),
                                HttpUtility.HtmlEncode(records.UserName),
                                HttpUtility.HtmlEncode(records.Note),
                                records.Id.ToString()
                            }
                        ).ToArray()
            };
            return Json(stockAdjustmentJsonData);
        }

        [Authorize(Policy = "CreatePolicy")]
        public async Task<IActionResult> Create()
        {
            var model = new StockAdjustmentCreateModel();

            var reasons = await _stockAdjustmentManagementService.GetStockAdjustmentReasonsAsync();
            var warehouses = await _warehouseManagementService.GetOrderedWarehousesAsync();

            model.SetReasons(reasons);
            model.SetWarhouses(warehouses);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CreatePolicy")]
        public async Task<IActionResult> Create(StockAdjustmentCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var stockAdjustment = _mapper.Map<StockAdjustment>(model);
                stockAdjustment.Id = Guid.NewGuid();
                stockAdjustment.UserName = User.Identity?.Name;

                try
                {
                    var stockAdjustmentItems = (from item in stockAdjustment.StockAdjustmentItems
                                                where item.AdjustedQuantity != 0
                                                select new StockAdjustmentItem
                                                {
                                                    Id = Guid.NewGuid(),
                                                    ItemId = item.ItemId,
                                                    IsIncrease = item.IsIncrease,
                                                    AdjustedQuantity = item.AdjustedQuantity,
                                                }).ToList();

                    if (stockAdjustmentItems.Count > 0)
                    {
                        await _stockAdjustmentManagementService
                            .CreateStockAdjustmentAsync(stockAdjustment, stockAdjustmentItems);
                    }

                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Stock transferred Successfully",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (InvalidOperationException ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = ex.Message,
                        Type = ResponseTypes.Danger
                    });
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Stock transfer failed",
                        Type = ResponseTypes.Danger
                    });
                }
            }
            else
            {
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Invalid form submission",
                    Type = ResponseTypes.Danger
                });
            }

            return RedirectToAction("Create");
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CreatePolicy")]
        public async Task<JsonResult> CreateStockAdjustmentReason([FromBody] StockAdjustmentReasonCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var reason = new StockAdjustmentReason
                {
                    Id = Guid.NewGuid(),
                    Name = model.ReasonName
                };
                
                await _stockAdjustmentManagementService
                            .CreateStockAdjustmentReasonAsync(reason);

                return Json(new
                {
                    success = true,
                    reasonId = reason.Id,
                    reasonName = reason.Name
                });

            }
            return Json(new { success = false });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> GetSearchedItemJsonData([FromBody] ItemAjaxSearch search)
        {
            var items = await _itemManagementService.GetItemsForStockTransferAsync(search.SearchItem, search.WarehouseId);

            var itemsJson = from item in items
                            select new
                            {
                                itemId = item.Id,
                                itemName = item.ItemName,
                                stockQuantity = item.ItemWarehouses?
                                                .FirstOrDefault()?
                                                .StockQuantity.ToString()
                            };

            return new JsonResult(itemsJson);
        }

        [Authorize(Policy = "DeletePolicy")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _stockAdjustmentManagementService.DeleteStockAdjustmentAsync(id);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Data deleted successfully",
                    Type = ResponseTypes.Success
                });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Data Delete failed",
                    Type = ResponseTypes.Danger
                });
            }
            return View();
        }
    }
}
