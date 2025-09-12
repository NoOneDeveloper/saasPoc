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
        private readonly string _userType;
        public SessionAuthorizeAttribute(string userType)
        {
            _userType = userType;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                                        .Any(em => em is Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute);

            if (hasAllowAnonymous)
                return;
            var userEmail = context.HttpContext.Session.GetString("UserEmail");
            var userType = context.HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(userEmail))
            {
                context.Result = new RedirectToActionResult("SignInBasic", "Authentication", null);
            }
            if (!string.Equals(userType, _userType, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Authentication", null);
                return;
            }
        }
    }
}
