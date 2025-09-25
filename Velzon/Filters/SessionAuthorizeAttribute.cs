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
        private readonly string[] _allowedRoles;

        // 🔑 Constructor ab multiple roles accept karega
        public SessionAuthorizeAttribute(params string[] roles)
        {
            _allowedRoles = roles;
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
                context.Result = new RedirectToActionResult("SignInB", "Authentication", null);
                return;
            }

            // 2️⃣ If login exists but role is not in allowed list → Access Denied
            if (!_allowedRoles.Any(r => string.Equals(r, userType, StringComparison.OrdinalIgnoreCase)))
            {
                context.Result = new RedirectToActionResult("Errors404Basic", "Authentication", null);
                return;
            }
        }
    }
}
