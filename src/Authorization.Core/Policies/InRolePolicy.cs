using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization.Policies
{
    /// <summary>
    /// This is intended for retrofitting existing applications that use roles.
    /// For new applications, ideally roles are not used.
    /// </summary>
    public class InRolePolicy : Policy
    {
        public override Task<bool> ExecuteAsync(ClaimsPrincipal user, params IPolicyContext[] args)
        {
            if (args == null || !args.Any())
            {
                return Task.FromResult(false);
            }

            var roles = args
                .Where(x => x.GetType() == typeof(InRolePolicyContext))
                .Cast<InRolePolicyContext>()
                .Select(x => x.Roles)
                .SelectMany(x => x.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                .Select(x => x.Trim());

            return Task.FromResult(user != null && roles.Any(x => user.IsInRole(x)));
        }
    }

    public class InRolePolicyContext : IPolicyContext
    {
        public InRolePolicyContext(string roles)
        {
            Roles = roles?.Trim();
        }

        public string Roles { get; set; }
    }
}
