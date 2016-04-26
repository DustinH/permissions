using Authorization.Policies;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Authorization.Tests.Policies
{
    public class IsAuthenticatedPolicyTests
    {
        [Fact]
        public async Task ExecuteAsync_ReturnsFalseWhenNotAuthenticated()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var result = await new IsAuthenticatedPolicy().ExecuteAsync(user);

            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsTrueWhenAuthenticated()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity("Forms"));

            var result = await new IsAuthenticatedPolicy().ExecuteAsync(user);

            Assert.True(result);
        }
    }
}
