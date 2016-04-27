using Authorization.Policies;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class InRolePolicyAttribute : PolicyAttribute
    {
        public InRolePolicyAttribute()
            : base(typeof(InRolePolicy)) { }

        public string Roles { get; set; }

        protected override async Task<bool> ExecutePolicy(ClaimsPrincipal user, Policy policy)
        {
            return await policy.ExecuteAsync(user, new InRolePolicyContext(Roles)).ConfigureAwait(false);
        }
    }
}
