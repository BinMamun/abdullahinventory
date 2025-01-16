using AutoMapper;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure.Extensions;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Admin.Models.CategoryModels;
using DevSkill.Inventory.Web.Areas.Admin.Models.ServiceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Super Admin, Admin")]
    public class ServiceController : Controller
    {
        private readonly ILogger<ServiceController> _logger;
        private readonly IMapper _mapper;
        private readonly IServiceManagementService _serviceManagementService;
        private readonly ITaxManagementService _taxManagementService;

        public ServiceController(
            ILogger<ServiceController> logger,
            IMapper mapper,
            IServiceManagementService serviceManagementService,
            ITaxManagementService taxManagementService
            )
        {
            _logger = logger;
            _mapper = mapper;
            _serviceManagementService = serviceManagementService;
            _taxManagementService = taxManagementService;
        }

        [Authorize(Policy = "ViewIndexPolicy")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetServiceJsonData([FromBody] ServiceListModel model)
        {
            var result = await _serviceManagementService.GetServicesAsync(
                model.PageIndex,
                model.PageSize,
                model.Search,
                model.FormatSortExpression("ServiceName", "Tax", "SellingPrice"));

            var serviceJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from records in result.data
                        select new string[]
                            {
                                HttpUtility.HtmlEncode(records.ServiceName),
                                HttpUtility.HtmlEncode(records.Tax?.Parcentage.ToString()),
                                HttpUtility.HtmlEncode(records.SellingPrice),
                                records.Id.ToString()
                            }
                        ).ToArray()
            };
            return Json(serviceJsonData);
        }

        [Authorize(Policy = "CreatePolicy")]
        public async Task<IActionResult> Create()
        {
            var model = new ServiceCreateModel();

            var taxList = await _taxManagementService.GetTaxListAsync();
            model.SetTaxValues(taxList);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CreatePolicy")]
        public async Task<IActionResult> Create(ServiceCreateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var service = _mapper.Map<Service>(model);
                    service.Id = Guid.NewGuid();

                    service.Tax = model.TaxId.HasValue ? await _taxManagementService.GetTaxAsync((Guid)model.TaxId) : null;

                    await _serviceManagementService.CreateServiceAsync(service);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Data created successfully",
                        Type = ResponseTypes.Success
                    });
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
            var taxList = await _taxManagementService.GetTaxListAsync();
            model.SetTaxValues(taxList);
            return View(model);
        }

        [Authorize(Policy = "EditPolicy")]
        public async Task<IActionResult> Update(Guid id)
        {
            var service = await _serviceManagementService.GetServiceAsync(id);
            var model = _mapper.Map<ServiceUpdateModel>(service);

            var taxList = await _taxManagementService.GetTaxListAsync();
            model.SetTaxValues(taxList);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "EditPolicy")]
        public async Task<IActionResult> Update(ServiceUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var service = await _serviceManagementService.GetServiceAsync(model.Id);
                    service = _mapper.Map(model, service);

                    service.Tax = model.TaxId.HasValue ? await _taxManagementService.GetTaxAsync((Guid)model.TaxId) : null;

                    await _serviceManagementService.UpdateServiceAsync(service);

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
            model.SetTaxValues(taxList);
            return View(model);
        }

        [Authorize(Policy = "DeletePolicy")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _serviceManagementService.DeleteServiceAsync(id);
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
