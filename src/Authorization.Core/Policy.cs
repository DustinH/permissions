using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization
{
    public abstract class Policy
    {
        public virtual string Name => GetType().Name;

        public abstract Task<bool> ExecuteAsync(ClaimsPrincipal user, params IPolicyContext[] args);
    }
}
