using AutoMapper;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure.Extensions;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Admin.Models.MeasurementUnitModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class MeasurementUnitController : Controller
    {
        private ILogger<MeasurementUnitController> _logger;
        private IMapper _mapper;
        private IMeasurementUnitManagementService _measurementUnitManagementService;

        public MeasurementUnitController(
            ILogger<MeasurementUnitController> logger,
            IMapper mapper,
            IMeasurementUnitManagementService measurementUnitManagementService
            )
        {
            _logger = logger;
            _mapper = mapper;
            _measurementUnitManagementService = measurementUnitManagementService;
        }

        [Authorize(Policy = "ViewIndexPolicy")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetMeasurementUnitJsonDataIndex([FromBody] MeasurementUnitListModel model)
        {
            var result = await _measurementUnitManagementService.GetMeasurementUnitsAsync(
                model.PageIndex,
                model.PageSize,
                model.Search,
                model.FormatSortExpression("Name", "Symbol"));

            var measurementUnitJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from records in result.data
                        select new string[]
                            {
                                HttpUtility.HtmlEncode(records.Name),
                                HttpUtility.HtmlEncode(records.Symbol),
                                records.Id.ToString()
                            }
                        ).ToArray()
            };
            return Json(measurementUnitJsonData);
        }

        [Authorize(Policy = "CreatePolicy")]
        public IActionResult Create()
        {
            var model = new MeasurementUnitCreateModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CreatePolicy")]
        public async Task<IActionResult> CreateAsync(MeasurementUnitCreateModel model)
        {
            var measurementUnit = _mapper.Map<MeasurementUnit>(model);
            measurementUnit.Id = Guid.NewGuid();

            if (ModelState.IsValid)
            {
                try
                {
                    await _measurementUnitManagementService.CreateMeasurementUnitAsync(measurementUnit);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Data Created successfully",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Data Created failed",
                        Type = ResponseTypes.Danger
                    });
                    return RedirectToAction("Create");
                }
            }
            return RedirectToAction("Create");
        }

        [Authorize(Policy = "EditPolicy")]
        public async Task<IActionResult> Update(Guid id)
        {
            var measurementUnit = await _measurementUnitManagementService.GetMeasurementUnitAsync(id);
            var model = _mapper.Map<MeasurementUnit>(measurementUnit);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "EditPolicy")]
        public async Task<IActionResult> Update(MeasurementUnitUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var measurementUnit = await _measurementUnitManagementService
                                            .GetMeasurementUnitAsync(model.Id);
                measurementUnit = _mapper.Map(model, measurementUnit);
                try
                {
                    await _measurementUnitManagementService.UpdateMeasurementUnitAsync(measurementUnit);

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
            return RedirectToAction("Update");
        }

        [Authorize(Policy = "DeletePolicy")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _measurementUnitManagementService.DeleteMeasurementUnitAsync(id);
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
                return RedirectToAction("Index");
            }
        }
    }
}
