using Authorization.Policies;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Authorization.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class InRolePolicyAttribute : AuthorizationFilterAttribute
    {
        public string Roles { get; set; }

        public override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var user = (ClaimsPrincipal)actionContext.RequestContext.Principal;

            var dependencyScope = actionContext.Request.GetDependencyScope();
            var policiesService = (PoliciesService)dependencyScope.GetService(typeof(PoliciesService));

            var isAuthenticated = await policiesService.ExecuteAsync<IsAuthenticatedPolicy>(user);

            if (!isAuthenticated)
            {
                HandleUnauthenticated(actionContext);
            }

            var isAuthorized = await policiesService.ExecuteAsync<InRolePolicy>(
                user,
                new InRolePolicyContext(Roles));

            if (!isAuthorized)
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(
                    HttpStatusCode.Forbidden,
                    "You are not authorized to access this resource.");
            }
        }

        private void HandleUnauthenticated(HttpActionContext actionContext)
        {
            if (actionContext == null)
                throw new ArgumentNullException(nameof(actionContext));

            actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(
                HttpStatusCode.Unauthorized,
                "You must be authenticated to access this resource.");
        }
    }
}
