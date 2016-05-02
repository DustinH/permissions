using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Authorization.Mvc
{
    public class CustomAttribute : FilterAttribute, IAuthorizationFilter
    {
        public CustomAttribute(Type abilityType)
        {
            AbilityType = abilityType;
        }

        public Type AbilityType { get; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }

            if (AbilityType == null)
            {
                throw new InvalidOperationException($"{nameof(AbilityType)} cannot be null.");
            }

            if (!typeof(Ability).IsAssignableFrom(AbilityType))
            {
                throw new InvalidOperationException(
                    $"{AbilityType} does not implement {nameof(Ability)}.");
            }

            var user = (ClaimsPrincipal)filterContext.HttpContext.User;

            var policy = DependencyResolver.Current?.GetService(AbilityType) as Ability;
            if (policy == null)
            {
                throw new InvalidOperationException($"{AbilityType} is not registered.");
            }

            var isAuthorized = AsyncHelpers.RunSync(() => ExecutePolicy(user, policy));

            if (!isAuthorized)
            {
                filterContext.Result = new HttpStatusCodeResult(
                    HttpStatusCode.Forbidden,
                    "You are not authorized to access this resource.");
            }
        }

        protected virtual async Task<bool> ExecutePolicy(ClaimsPrincipal user, Ability ability)
        {
            return await ability.ExecuteAsync(user).ConfigureAwait(false);
        }
    }
}
