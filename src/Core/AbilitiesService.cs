using Autofac;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization
{
    public class AbilitiesService
    {
        private readonly ILifetimeScope scope;

        public AbilitiesService(ILifetimeScope scope)
        {
            this.scope = scope;
        }

        public virtual async Task<bool> ExecuteAsync(Type policyType, ClaimsPrincipal user, params IAbilityContext[] args)
        {
            var policy = (Ability)scope.Resolve(policyType);

            return await policy.ExecuteAsync(user, args);
        }

        public virtual async Task<bool> ExecuteAsync<T>(ClaimsPrincipal user, params IAbilityContext[] args)
            where T : Ability
        {
            return await ExecuteAsync(typeof(T), user, args);
        }
    }
}
