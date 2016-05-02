using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization.Abilities
{
    public class HasClaimAbility : Ability
    {
        public override Task<bool> ExecuteAsync(ClaimsPrincipal user, params IAbilityContext[] args)
        {
            var argsExist = args != null || args.Any();

            Func<bool> hasClaim = () =>
            {
                var claimTypes = args
                    .Where(x => x.GetType() == typeof(HasClaimAbilityContext))
                    .Cast<HasClaimAbilityContext>()
                    .Select(x => x.ClaimType)
                    .SelectMany(x => x.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                    .Select(x => x.Trim());

                return claimTypes.Any(x => user.Claims.Any(y => x == y.Type));
            };

            return Task.FromResult(user != null && argsExist && hasClaim());
        }
    }

    public class HasClaimAbilityContext : IAbilityContext
    {
        public HasClaimAbilityContext(string claimType)
        {
            ClaimType = claimType;
        }

        public string ClaimType { get; set; }
    }
}
