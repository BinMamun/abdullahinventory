using AutoMapper;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure.Extensions;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Admin.Models.WarehouseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class WarehouseController : Controller
    {
        private readonly ILogger<WarehouseController> _logger;
        private readonly IMapper _mapper;
        private readonly IWarehouseManagementService _warehouseManagementService;

        public WarehouseController(
            ILogger<WarehouseController> logger,
            IMapper mapper,
            IWarehouseManagementService warehouseManagementService
            )
        {
            _logger = logger;
            _mapper = mapper;
            _warehouseManagementService = warehouseManagementService;
        }

        [Authorize(Policy = "ViewIndexPolicy")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetWarehouseJsonData([FromBody] WarehouseListModel model)
        {
            var result = await _warehouseManagementService.GetWarehousesAsync(
                model.PageIndex,
                model.PageSize,
                model.Search,
                model.FormatSortExpression("Name"));

            var warehouseJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from records in result.data
                        select new string[]
                            {
                                HttpUtility.HtmlEncode(records.Name),
                                records.Id.ToString()
                            }
                        ).ToArray()
            };
            return Json(warehouseJsonData);
        }

        [Authorize(Policy = "CreatePolicy")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CreatePolicy")]
        public async Task<IActionResult> Create(WarehouseCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var warehouse = _mapper.Map<Warehouse>(model);
                warehouse.Id = Guid.NewGuid();

                try
                {
                    await _warehouseManagementService.CreateWarehouseAsync(warehouse);

                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Data created Successfully",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Data creation failed",
                        Type = ResponseTypes.Danger
                    });
                }
            }
            return View(model);
        }

        [Authorize(Policy = "EditPolicy")]
        public async Task<IActionResult> Update(Guid id)
        {
            var warehouse = await _warehouseManagementService.GetWarehouseAsync(id);
            var model = _mapper.Map<WarehouseUpdateModel>(warehouse);
            return View(model);
        }

        [HttpPost,ValidateAntiForgeryToken, Authorize(Policy = "EditPolicy")]
        public async Task<IActionResult> Update(WarehouseUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var warehouse = await _warehouseManagementService.GetWarehouseAsync(model.Id);

                warehouse = _mapper.Map(model, warehouse);

                try
                {
                    await _warehouseManagementService.UpdateWarehouseAsync(warehouse);

                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Data updated Successfully",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Data update failed",
                        Type = ResponseTypes.Danger
                    });
                }
            }
            return View(model);
        }

        [Authorize(Policy = "DeletePolicy")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _warehouseManagementService.DeleteWarehouseAsync(id);
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
