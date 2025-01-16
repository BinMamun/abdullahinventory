using AutoMapper;
using DevSkill.Inventory.Infrastructure.Extensions;
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Admin.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdministrationPolicy")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UserController(
            ILogger<UserController> logger,
            IMapper mapper,
        UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager
            )
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetUsersJsonData([FromBody] UserListModel model)
        {
            var users = await _userManager.Users.ToListAsync();

            var userList = new List<UserListModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserListModel
                {
                    Id = user.Id,
                    Name = $"{user.FirstName} {user.LastName}",
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = string.Join(", ", roles),
                });
            }

            int total = users.Count;
            int totalDisplay = model.PageSize;

            (IList<UserListModel> data, int total, int totalDisplay) result = (userList, total, totalDisplay);

            //(IList<UserListModel> data, int total, int totalDisplay) result = (await Task.WhenAll(users.Select(
            //                async user =>
            //                new UserListModel
            //                {
            //                    Id = user.Id,
            //                    Name = $"{user.FirstName} {user.LastName}",
            //                    UserName = user.UserName,
            //                    Email = user.Email,
            //                    Role = string.Join(", ", await _userManager.GetRolesAsync(user)),
            //                })),
            //                users.Count,
            //                model.PageSize);

            var usersJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from records in result.data
                        select new string[]
                            {
                                HttpUtility.HtmlEncode(records.Name),
                                HttpUtility.HtmlEncode(records.UserName),
                                HttpUtility.HtmlEncode(records.Email),
                                HttpUtility.HtmlEncode(records.Role),
                                records.Id.ToString()
                            }
                        ).ToArray()
            };
            return Json(usersJsonData);
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var model = _mapper.Map<UserUpdateModel>(user);

            model.Roles = _roleManager.Roles.ToList();

            model.UserRoles = (List<string>)await _userManager.GetRolesAsync(user);

            var userClaims =  await _userManager.GetClaimsAsync(user);
            
            model.Permissions = model.GetUserPermisssions(userClaims);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(model.Id.ToString());

                    user = _mapper.Map(model, user);

                    var currentRoles = await _userManager.GetRolesAsync(user);
                    var currentClaims = await _userManager.GetClaimsAsync(user);

                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.RemoveClaimsAsync(user, currentClaims);


                    var newRoles = _roleManager.Roles
                                    .Where(role => model.UserRoles.Contains(role.Name))
                                    .Select(role => role.Name).ToList();

                    await _userManager.AddToRolesAsync(user, newRoles);

                    var newClaims = model.Permissions
                                    .Where(claim => claim.ClaimValue.Equals(true))
                                    .Select(claim => new Claim(claim.ClaimType, "true"));

                    await _userManager.AddClaimsAsync(user, newClaims);

                    await _userManager.UpdateAsync(user);

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
                        Message = "Failed to update data",
                        Type = ResponseTypes.Danger
                    });
                    return RedirectToAction("Update");
                }
            }
            else
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Invalid form submission",
                    Type = ResponseTypes.Danger
                });
                return RedirectToAction("Update");
            }
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());

                if (user != null)
                {
                    await _userManager.DeleteAsync(user);

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
