using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization.Policies
{
    public class IsAuthenticatedPolicy : Policy
    {
        public override Task<bool> ExecuteAsync(ClaimsPrincipal user, params IPolicyContext[] args)
        {
            return Task.FromResult(user != null && user.Identity.IsAuthenticated);
        }
    }
}
