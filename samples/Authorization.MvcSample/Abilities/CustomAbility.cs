using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization.MvcSample.Abilities
{
    public class CustomAbility : Ability
    {
        public override Task<bool> ExecuteAsync(ClaimsPrincipal user, params IAbilityContext[] args)
        {
            return Task.FromResult(user.Identity.Name == Startup.StuntmanOptions.Users.Single().Name);
        }
    }
}
