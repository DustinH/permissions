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
    public class CustomAttribute : AuthorizationFilterAttribute
    {
        public CustomAttribute(Type abilityType)
        {
            AbilityType = abilityType;
        }

        public Type AbilityType { get; }

        protected virtual async Task<bool> ExecutePolicy(ClaimsPrincipal user, Ability ability)
        {
            return await ability.ExecuteAsync(user).ConfigureAwait(false);
        }

        public sealed override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (AbilityType == null)
            {
                throw new InvalidOperationException($"{nameof(AbilityType)} cannot be null.");
            }

            if (!typeof(Ability).IsAssignableFrom(AbilityType))
            {
                throw new InvalidOperationException(
                    $"{AbilityType} does not implement {nameof(Ability)}.");
            }

            var user = (ClaimsPrincipal)actionContext.RequestContext.Principal;

            var dependencyScope = actionContext.Request.GetDependencyScope();

            var policy = dependencyScope.GetService(AbilityType) as Ability;
            if (policy == null)
            {
                throw new InvalidOperationException($"{AbilityType} is not registered.");
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
