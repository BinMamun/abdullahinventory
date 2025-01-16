using Autofac.Core;
using AutoMapper;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure.Extensions;
using DevSkill.Inventory.Infrastructure.Utilities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Admin.Models.ItemModels;
using DevSkill.Inventory.Web.Areas.Admin.Models.ServiceModels;
using DevSkill.Inventory.Web.Areas.Admin.Models.StockListModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class ItemController : Controller
    {
        private readonly ILogger<ItemController> _logger;
        private readonly IMapper _mapper;
        private readonly IItemManagementService _itemManagementService;
        private readonly ITaxManagementService _taxManagementService;
        private readonly ICategoryManagementService _categoryManagementService;
        private readonly IMeasurementUnitManagementService _measurementUnitManagementService;
        private readonly IImageServiceUtility _imageServiceUtility;
        private readonly IWarehouseManagementService _warehouseManagementService;


        public ItemController(
            ILogger<ItemController> logger,
            IMapper mapper,
            IItemManagementService itemManagementService,
            ITaxManagementService taxManagementService,
            ICategoryManagementService categoryManagementService,
            IMeasurementUnitManagementService measurementUnitManagementService,
            IImageServiceUtility imageServiceUtility,
            IWarehouseManagementService warehouseManagementService
            )
        {
            _logger = logger;
            _mapper = mapper;
            _itemManagementService = itemManagementService;
            _taxManagementService = taxManagementService;
            _categoryManagementService = categoryManagementService;
            _measurementUnitManagementService = measurementUnitManagementService;
            _imageServiceUtility = imageServiceUtility;
            _warehouseManagementService = warehouseManagementService;
        }

        [Authorize(Policy = "ViewIndexPolicy")]
        public async Task<IActionResult> Index()
        {
            var model = new ItemListModel();

            var categories = await _categoryManagementService.GetCategoriesAsync();
            var warehouses = await _warehouseManagementService.GetOrderedWarehousesAsync();

            model.SetCategories(categories);
            model.SetWarehouses(warehouses);

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> GetItemJsonData([FromBody] ItemListModel model)
        {
            var result = await _itemManagementService.GetItemsAsync(
                model.PageIndex,
                model.PageSize,
                model.SearchItem,
                model.FormatSortExpression("Id", "Picture", "ItemName", "Barcode", "Category", "Price", "Tax", "MinimumStockQuantity", "TotalStockQuantity", "IsActive", "Id"));


            var itemJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from records in result.data
                        select new string[]
                            {
                                records.Id.ToString(),
                                HttpUtility.HtmlEncode(records.Picture),
                                HttpUtility.HtmlEncode(records.ItemName),
                                HttpUtility.HtmlEncode(records.Barcode),
                                HttpUtility.HtmlEncode(records.Category),
                                HttpUtility.HtmlEncode(records.Price),
                                HttpUtility.HtmlEncode(records.Tax),
                                HttpUtility.HtmlEncode(records.MinimumStockQuantity),
                                HttpUtility.HtmlEncode($"{records.TotalStockQuantity} " +
                                                        $"{records.MeasurementUnit}"),
                                //HttpUtility.HtmlEncode(records.TotalStockQuantity.HasValue ?
                                //$"{records.TotalStockQuantity} {records.MeasurementUnit}" : ""),
                                HttpUtility.HtmlEncode(records.IsActive.Equals(true) ?
                                                        "Active" : "Inactive"),
                                records.Id.ToString()
                            }
                        ).ToArray()
            };
            return Json(itemJsonData);
        }

        [Authorize(Policy = "CreatePolicy")]
        public async Task<IActionResult> Create()
        {
            var model = new ItemCreateModel();

            var taxList = await _taxManagementService.GetTaxListAsync();
            var categoryList = await _categoryManagementService.GetCategoriesAsync();
            var unitList = await _measurementUnitManagementService.GetMeasurementUnitsAsync();
            var warehouses = await _warehouseManagementService.GetWarehouseListAsync();

            model.SetTaxValues(taxList);
            model.SetCategories(categoryList);
            model.SetMeasurementUnits(unitList);
            model.SetItemWahouses(warehouses);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CreatePolicy")]
        public async Task<IActionResult> Create(ItemCreateModel model, string action)
        {
            if (ModelState.IsValid)
            {
                var item = _mapper.Map<Item>(model);
                item.Id = Guid.NewGuid();
                item.Picture = await _imageServiceUtility.UploadImage(model.Picture);
                item.Tax = model.TaxId.HasValue ? await _taxManagementService.GetTaxAsync((Guid)model.TaxId) : null;

                item.Category = model.CategoryId.HasValue ? await _categoryManagementService.GetCategoryAsync((Guid)model.CategoryId) : null;

                item.MeasurementUnit = await _measurementUnitManagementService
                    .GetMeasurementUnitAsync(model.MeasurementUnitId);

                try
                {

                    await _itemManagementService.CreateItemAsync(item);

                    var itemWarehouses = (from w in model.Warehouses
                                          where w.StockQuantity > 0
                                          select new ItemWarehouse
                                          {
                                              ItemId = item.Id,
                                              WarehouseId = w.WarehouseId,
                                              StockQuantity = w.StockQuantity,
                                              AsOfDate = model.AsOfDate,
                                              CostPerUnit = model.CostPerUnit
                                          }).ToList();

                    if (!itemWarehouses.Count.Equals(0))
                    {
                        await _itemManagementService.CreateItemWarehouseAsync(itemWarehouses);
                    }

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Data created successfully",
                        Type = ResponseTypes.Success
                    });

                    if (action == "saveAndNew")
                        return RedirectToAction("Create");
                    else
                        return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Data creation failed",
                        Type = ResponseTypes.Danger
                    });
                    return RedirectToAction("Create");
                }
            }
            else
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Form submission is not valid",
                    Type = ResponseTypes.Danger
                });

                var taxList = await _taxManagementService.GetTaxListAsync();
                var categoryList = await _categoryManagementService.GetCategoriesAsync();
                var unitList = await _measurementUnitManagementService.GetMeasurementUnitsAsync();
                var warehouses = await _warehouseManagementService.GetWarehouseListAsync();

                model.SetTaxValues(taxList);
                model.SetCategories(categoryList);
                model.SetMeasurementUnits(unitList);
                model.SetItemWahouses(warehouses);
                return View(model);
            }
        }

        [Authorize(Policy = "EditPolicy")]
        public async Task<IActionResult> Update(Guid id)
        {
            var item = await _itemManagementService.GetItemAsync(id);
            var model = _mapper.Map<ItemUpdateModel>(item);

            model.PicturePath = !string.IsNullOrWhiteSpace(item.Picture) ? item.Picture : null;

            var taxList = await _taxManagementService.GetTaxListAsync();
            var categoryList = await _categoryManagementService.GetCategoriesAsync();
            var unitList = await _measurementUnitManagementService.GetMeasurementUnitsAsync();
            var warehouses = await _warehouseManagementService.GetWarehouseListAsync();

            model.SetTaxValues(taxList);
            model.SetCategories(categoryList);
            model.SetMeasurementUnits(unitList);
            model.GetItemWahouses(warehouses);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "EditPolicy")]
        public async Task<IActionResult> Update(ItemUpdateModel model)
        {

            if (ModelState.IsValid)
            {
                var item = await _itemManagementService.GetItemAsync(model.Id);

                item = _mapper.Map(model, item);

                if (model.Picture == null)
                {
                    item.Picture = model.PicturePath;
                }
                else
                {
                    item.Picture = await _imageServiceUtility.UploadImage(model.Picture);
                    if (model.PicturePath != null)
                    {
                        await _imageServiceUtility.DeleteImage(model.PicturePath);
                    }
                }

                item.Tax = model.TaxId.HasValue ? await _taxManagementService.GetTaxAsync((Guid)model.TaxId) : null;

                item.Category = model.CategoryId.HasValue ? await _categoryManagementService.GetCategoryAsync((Guid)model.CategoryId) : null;

                item.MeasurementUnit = await _measurementUnitManagementService
                    .GetMeasurementUnitAsync(model.MeasurementUnitId);

                try
                {
                    await _itemManagementService.UpdateItemAsync(item);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Data updated successfully",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Data update failed",
                        Type = ResponseTypes.Danger
                    });
                    return RedirectToAction("Update");
                }
            }

            var taxList = await _taxManagementService.GetTaxListAsync();
            var categoryList = await _categoryManagementService.GetCategoriesAsync();
            var unitList = await _measurementUnitManagementService.GetMeasurementUnitsAsync();

            model.SetTaxValues(taxList);
            model.SetCategories(categoryList);
            model.SetMeasurementUnits(unitList);
            return View(model);
        }

        [Authorize(Policy = "DeletePolicy")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _itemManagementService.DeleteItemAsync(id);
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

        [Authorize(Policy = "DeleteAllPolicy")]
        public async Task<IActionResult> DeleteAll(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "No data selected",
                    Type = ResponseTypes.Warning
                });
                return RedirectToAction("Index");
            }
            try
            {
                var selectedItemIds = JsonSerializer.Deserialize<List<Guid>>(id);

                await _itemManagementService.DeleteMultipleItemsAsync(selectedItemIds);

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
