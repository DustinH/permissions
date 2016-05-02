using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Authorization.Abilities;

namespace Authorization.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AbilityAttribute : CustomAttribute
    {
        public AbilityAttribute(string claim)
            : base(typeof(AbilityAttribute))
        {
            Claim = claim;
        }

        public string Claim { get; }

        protected override async Task<bool> ExecutePolicy(ClaimsPrincipal user, Ability ability)
        {
            return await ability.ExecuteAsync(user, new HasClaimAbilityContext(Claim)).ConfigureAwait(false);
        }
    }
}
