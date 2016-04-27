using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Authorization.Mvc
{
    public class PolicyAttribute : FilterAttribute, IAuthorizationFilter
    {
        public PolicyAttribute(Type policyType)
        {
            PolicyType = policyType;
        }

        public Type PolicyType { get; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }

            if (PolicyType == null)
            {
                throw new InvalidOperationException($"{nameof(PolicyType)} cannot be null.");
            }

            if (!typeof(Policy).IsAssignableFrom(PolicyType))
            {
                throw new InvalidOperationException(
                    $"{PolicyType} does not implement {nameof(Policy)}.");
            }

            var user = (ClaimsPrincipal)filterContext.HttpContext.User;

            var policy = DependencyResolver.Current?.GetService(PolicyType) as Policy;
            if (policy == null)
            {
                throw new InvalidOperationException($"{PolicyType} is not registered.");
            }

            var isAuthorized = AsyncHelpers.RunSync(() => ExecutePolicy(user, policy));

            if (!isAuthorized)
            {
                filterContext.Result = new HttpStatusCodeResult(
                    HttpStatusCode.Forbidden,
                    "You are not authorized to access this resource.");
            }
        }

        protected virtual async Task<bool> ExecutePolicy(ClaimsPrincipal user, Policy policy)
        {
            return await policy.ExecuteAsync(user).ConfigureAwait(false);
        }
    }
}
