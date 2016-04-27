using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Authorization.Mvc
{
    public class AuthenticateAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext actionContext)
        {
            if (actionContext == null)
                throw new ArgumentNullException(nameof(actionContext));

            if (SkipAuthorization(actionContext))
            {
                return;
            }

            var user = actionContext.HttpContext?.User;
            if (user?.Identity == null || !user.Identity.IsAuthenticated)
            {
                actionContext.Result = new HttpStatusCodeResult(
                    HttpStatusCode.Unauthorized,
                    "You must be authenticated to access this resource.");
            }
        }

        private static bool SkipAuthorization(AuthorizationContext actionContext)
        {
            var actionDescriptor = actionContext.ActionDescriptor;
            return actionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionDescriptor.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }
}
