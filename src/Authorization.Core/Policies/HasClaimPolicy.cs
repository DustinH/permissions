using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization.Policies
{
    public class HasClaimPolicy : Policy
    {
        public override Task<bool> ExecuteAsync(ClaimsPrincipal user, params IPolicyContext[] args)
        {
            var argsExist = args != null || args.Any();

            Func<bool> hasClaim = () =>
            {
                var claimTypes = args
                    .Where(x => x.GetType() == typeof(HasClaimPolicyContext))
                    .Cast<HasClaimPolicyContext>()
                    .Select(x => x.ClaimType)
                    .SelectMany(x => x.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                    .Select(x => x.Trim());

                return claimTypes.Any(x => user.Claims.Any(y => x == y.Type));
            };

            return Task.FromResult(user != null && argsExist && hasClaim());
        }
    }

    public class HasClaimPolicyContext : IPolicyContext
    {
        public HasClaimPolicyContext(string claimType)
        {
            ClaimType = claimType;
        }

        public string ClaimType { get; set; }
    }
}
