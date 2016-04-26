using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization.Sample.Policies
{
    public class CustomPolicy : Policy
    {
        public override Task<bool> ExecuteAsync(ClaimsPrincipal user, params IPolicyContext[] args)
        {
            return Task.FromResult(user.Identity.Name == Startup.StuntmanOptions.Users.Single().Name);
        }
    }
}
