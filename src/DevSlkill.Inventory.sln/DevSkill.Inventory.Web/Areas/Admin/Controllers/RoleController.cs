using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Web.Areas.Admin.Models.UserModels;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using DevSkill.Inventory.Web.Areas.Admin.Models.RoleModels;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using DevSkill.Inventory.Infrastructure.Extensions;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdministrationPolicy")]
    public class RoleController : Controller
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IMapper _mapper;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public RoleController(
            ILogger<RoleController> logger,
            IMapper mapper,
            RoleManager<ApplicationRole> roleManager
            )
        {
            _logger = logger;
            _mapper = mapper;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetRolesJsonData([FromBody] RoleListModel model)
        {
            var result = _roleManager.Roles;

            var rolesJsonData = new
            {
                recordsTotal = result.Count(),
                recordsFiltered = result.Count(),
                data = (from records in result
                        select new string[]
                            {
                                HttpUtility.HtmlEncode(records.Name),
                                records.Id.ToString()
                            }
                        ).ToArray()
            };
            return Json(rolesJsonData);
        }

        public IActionResult Create()
        {
            var model = new RoleCreateModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleUpdateModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var role = new ApplicationRole
                    {
                        Id = Guid.NewGuid(),
                        Name = model.Name,
                        NormalizedName = model.Name.ToUpper(),
                        ConcurrencyStamp = DateTime.Now.Ticks.ToString()
                    };
                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "Data created successfully",
                            Type = ResponseTypes.Success
                        });
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var errors = result.Errors.Select(x => x.Description);
                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = string.Join(" ", errors),
                            Type = ResponseTypes.Danger
                        });
                        return RedirectToAction("Create");
                    }
                }
            }
            catch
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Data creation failed",
                    Type = ResponseTypes.Danger
                });
                return RedirectToAction("Create");
            }
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            var model = _mapper.Map<RoleUpdateModel>(role);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(RoleUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(model.Id.ToString());

                    if (role != null)
                    {
                        role.Id = model.Id;
                        role.Name = model.Name;

                        await _roleManager.UpdateAsync(role);

                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "Data updated successfully",
                            Type = ResponseTypes.Success
                        });
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Data update failed",
                        Type = ResponseTypes.Danger
                    });
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());

                if (role != null)
                {
                    await _roleManager.DeleteAsync(role);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Data deleted successfully",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
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
            return RedirectToAction("Index");
        }
    }
}
