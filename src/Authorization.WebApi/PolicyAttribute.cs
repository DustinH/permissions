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
    public class PolicyAttribute : AuthorizationFilterAttribute
    {
        public PolicyAttribute(Type policyType)
        {
            PolicyType = policyType;
        }

        public Type PolicyType { get; }

        protected virtual async Task<bool> ExecutePolicy(ClaimsPrincipal user, Policy policy)
        {
            return await policy.ExecuteAsync(user).ConfigureAwait(false);
        }

        public sealed override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (PolicyType == null)
            {
                throw new InvalidOperationException($"{nameof(PolicyType)} cannot be null.");
            }

            if (!typeof(Policy).IsAssignableFrom(PolicyType))
            {
                throw new InvalidOperationException(
                    $"{PolicyType} does not implement {nameof(Policy)}.");
            }

            var user = (ClaimsPrincipal)actionContext.RequestContext.Principal;

            var dependencyScope = actionContext.Request.GetDependencyScope();

            var policy = dependencyScope.GetService(PolicyType) as Policy;
            if (policy == null)
            {
                throw new InvalidOperationException($"{PolicyType} is not registered.");
            }

            var isAuthorized = await ExecutePolicy(user, policy);

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
