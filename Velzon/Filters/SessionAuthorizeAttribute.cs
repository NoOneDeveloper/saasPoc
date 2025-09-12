using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace YourWebProject.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
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

            // 1️⃣ If no login → go to SignIn
            if (string.IsNullOrEmpty(userEmail))
            {
                context.Result = new RedirectToActionResult("SignInBasic", "Authentication", null);
                return; // stop further execution
            }

            // 2️⃣ If login exists but wrong role → Access Denied
            if (!string.Equals(userType, _userType, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new RedirectToActionResult("Errors404Basic", "Authentication", null);
                return;
            }
        }
    }
}
