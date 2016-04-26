using Autofac;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization
{
    public class PoliciesService
    {
        private readonly ILifetimeScope scope;

        public PoliciesService(ILifetimeScope scope)
        {
            this.scope = scope;
        }

        public virtual async Task<bool> ExecuteAsync(Type policyType, ClaimsPrincipal user, params IPolicyContext[] args)
        {
            var policy = (Policy)scope.Resolve(policyType);

            return await policy.ExecuteAsync(user, args);
        }

        public virtual async Task<bool> ExecuteAsync<T>(ClaimsPrincipal user, params IPolicyContext[] args)
            where T : Policy
        {
            return await ExecuteAsync(typeof(T), user, args);
        }
    }
}
