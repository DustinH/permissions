using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Authorization.Abilities;

namespace Authorization.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RoleAttribute : CustomAttribute
    {
        public RoleAttribute(string role)
            : base(typeof(InRoleAbility))
        {
            Role = role;
        }

        public string Role { get; }

        protected override async Task<bool> ExecutePolicy(ClaimsPrincipal user, Ability ability)
        {
            return await ability.ExecuteAsync(user, new InRoleAbilityContext(Role)).ConfigureAwait(false);
        }
    }
}
