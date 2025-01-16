using AutoMapper;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure.Extensions;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Admin.Models.CategoryModels;
using DevSkill.Inventory.Web.Areas.Admin.Models.ServiceModels;
using DevSkill.Inventory.Web.Areas.Admin.Models.StockTransferModels;
using Inventory.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class StockTransferController : Controller
    {
        private readonly ILogger<StockTransferController> _logger;
        private readonly IMapper _mapper;
        private readonly IStockTransferManagementService _stockTransferManagementService;
        private readonly IWarehouseManagementService _warehouseManagementService;
        private readonly IItemManagementService _itemManagementService;
        public StockTransferController(
            ILogger<StockTransferController> logger,
            IMapper mapper,
            IStockTransferManagementService stockTransferManagementService,
            IWarehouseManagementService warehouseManagementService,
            IItemManagementService itemManagementService
            )
        {
            _logger = logger;
            _mapper = mapper;
            _stockTransferManagementService = stockTransferManagementService;
            _warehouseManagementService = warehouseManagementService;
            _itemManagementService = itemManagementService;
        }

        [Authorize(Policy = "ViewIndexPolicy")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<JsonResult> GetStockTransferJsonData([FromBody] StockTransferListModel model)
        {
            var result = await _stockTransferManagementService.GetStockTransfersAsync(
                model.PageIndex,
                model.PageSize,
                model.Search,
                model.FormatSortExpression("TransferDate", "FromWarehouse.Name", "ToWarehouse.Name", "Note"));

            var stockTransferJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from records in result.data
                        select new string[]
                            {
                                HttpUtility.HtmlEncode(records.TransferDate.ToString("dd-MM-yyyy")),
                                HttpUtility.HtmlEncode(records.FromWarehouse.Name),
                                HttpUtility.HtmlEncode(records.ToWarehouse.Name),
                                HttpUtility.HtmlEncode(records.UserName),
                                HttpUtility.HtmlEncode(records.Note),
                                records.Id.ToString()
                            }
                        ).ToArray()
            };
            return Json(stockTransferJsonData);
        }

        [HttpPost]
        public async Task<JsonResult> GetStockTransferItemsJsonData([FromBody] Guid id)
        {
            var result = await _stockTransferManagementService.GetStockTransferItemsAsync(id);

            var stockTransferWithItemsJsonData = new
            {
                data = (from records in result.StockTransferItems
                        select new string[]
                            {
                                HttpUtility.HtmlEncode(records.Item.ItemName),
                                HttpUtility.HtmlEncode($"{records.TransferQuantity} {records.Item.MeasurementUnit.Symbol}" ),
                            }
                        ).ToArray()
            };
            return Json(stockTransferWithItemsJsonData);
        }

        [Authorize(Policy = "CreatePolicy")]
        public async Task<IActionResult> Create()
        {
            var model = new StockTransferCreateModel();

            var warehouses = await _warehouseManagementService.GetOrderedWarehousesAsync();
            model.SetWarhouseValues(warehouses);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CreatePolicy")]
        public async Task<IActionResult> Create(StockTransferCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var stockTransfer = _mapper.Map<StockTransfer>(model);
                stockTransfer.Id = Guid.NewGuid();
                stockTransfer.UserName = User.Identity?.Name;

                try
                {
                    var stockTransferItems = (from item in stockTransfer.StockTransferItems
                                              where item.TransferQuantity > 0
                                              select new StockTransferItem
                                              {
                                                  Id = Guid.NewGuid(),
                                                  ItemId = item.ItemId,
                                                  TransferQuantity = item.TransferQuantity,
                                              }).ToList();

                    if (stockTransferItems.Count > 0)
                    {
                        await _stockTransferManagementService.CreateStockTransferAsync(stockTransfer, stockTransferItems);
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

        [HttpPost]
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
                await _stockTransferManagementService.DeleteStockTransferAsync(id);
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