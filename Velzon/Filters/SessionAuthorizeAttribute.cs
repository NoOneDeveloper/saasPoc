using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace YourWebProject.Filters
{
    public class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                                        .Any(em => em is Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute);

            if (hasAllowAnonymous)
                return;
            var userEmail = context.HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                context.Result = new RedirectToActionResult("SignInBasic", "Authentication", null);
            }
        }
    }
}
