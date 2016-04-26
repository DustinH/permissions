using Authorization.Policies;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ClaimPolicyAttribute : PolicyAttribute
    {
        public ClaimPolicyAttribute() : base(typeof(ClaimPolicyAttribute))
        {
        }

        public string Claim { get; set; }

        protected override async Task<bool> ExecutePolicy(ClaimsPrincipal user, Policy policy)
        {
            return await policy.ExecuteAsync(
                user,
                new HasClaimPolicyContext(Claim))
                .ConfigureAwait(false);
        }
    }
}
