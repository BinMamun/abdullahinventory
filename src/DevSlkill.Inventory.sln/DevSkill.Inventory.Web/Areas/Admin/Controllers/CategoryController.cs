using AutoMapper;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Admin.Models.CategoryModels;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using DevSkill.Inventory.Infrastructure.Extensions;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryManagementService _categoryManagementService;
        private readonly IMapper _mapper;

        public CategoryController(
            ILogger<CategoryController> logger,
            ICategoryManagementService categoryManagementService,
            IMapper mapper
            )
        {
            _logger = logger;
            _categoryManagementService = categoryManagementService;
            _mapper = mapper;
        }

        [Authorize(Policy = "ViewIndexPolicy")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetCategoryJsonData([FromBody] CategoryListModel model)
        {
            var result = await _categoryManagementService.GetCategoriesAsync(
                model.PageIndex,
                model.PageSize,
                model.Search,
                model.FormatSortExpression("Name", "Description"));

            var categoryJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from records in result.data
                        select new string[]
                            {
                                HttpUtility.HtmlEncode(records.Name),
                                HttpUtility.HtmlEncode(records.Description),
                                records.Id.ToString()
                            }
                        ).ToArray()
            };
            return Json(categoryJsonData);
        }

        [Authorize(Policy = "CreatePolicy")]
        public IActionResult Create()
        {
            var model = new CategoryCreateModel();            
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CreatePolicy")]
        public async Task<IActionResult> Create(CategoryCreateModel model)
        {
            var category = _mapper.Map<ItemCategory>(model);
            category.Id = Guid.NewGuid();
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryManagementService.CreateItemCategoryAsync(category);

                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Category created Successfully",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Catgory creation failed",
                        Type = ResponseTypes.Danger
                    });
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize(Policy = "EditPolicy")]
        public async Task<IActionResult> Update(Guid id)
        {
            var category = await _categoryManagementService.GetCategoryAsync(id);
            var model = _mapper.Map<CategoryUpdateModel>(category);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "EditPolicy")]
        public async Task<IActionResult> Update(CategoryUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var category = await _categoryManagementService.GetCategoryAsync(model.Id);
                category = _mapper.Map(model, category);

                try
                {
                    await _categoryManagementService.UpdateCategoryAsync(category);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Category updated successfully",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Category update failed",
                        Type = ResponseTypes.Danger
                    });
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize(Policy = "DeletePolicy")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _categoryManagementService.DeleteCategoryAsync(id);
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
