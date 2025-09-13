using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace YourWebProject.Filters
{
    public class GlobalSessionAuthorizeAttribute : ActionFilterAttribute
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
                context.Result = new RedirectToActionResult("SignIn", "Authentication", null);
            }
        }
    }
}
