using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Utilities
{
    public class RedirectIfLoggedin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            if(user.Identity?.IsAuthenticated == true)
            {
                context.Result = new RedirectToActionResult("Index", "Dashboard", new {area ="Admin"});
            }
            base.OnActionExecuting(context);
        }
    }
}