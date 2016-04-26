using Authorization.Policies;
using Autofac;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Authorization.Tests.Policies
{
    public class HasClaimPolicyTests : IDisposable
    {
        private readonly IContainer container;
        private readonly HasClaimPolicy sut;

        public HasClaimPolicyTests()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<PoliciesModule>();

            container = builder.Build();

            sut = container.Resolve<HasClaimPolicy>();
        }

        [Fact]
        public async Task ExecuteAsync_DoesNotThrowForNullArgs()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            await sut.ExecuteAsync(user);
        }

        [Fact]
        public async Task ExecuteAsync_HasClaim()
        {
            const string TestClaim = "TestClaim";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(TestClaim, "abc")
            }));

            var hasClaim = await sut.ExecuteAsync(
                user,
                new HasClaimPolicyContext(TestClaim));

            Assert.True(hasClaim);
        }

        [Fact]
        public async Task ExecuteAsync_DoesNotHaveClaim()
        {
            const string TestClaim = "TestClaim";

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var hasClaim = await sut.ExecuteAsync(
                user,
                new HasClaimPolicyContext(TestClaim));

            Assert.False(hasClaim);
        }

        [Fact]
        public async Task ExecuteAsync_HasMultipleClaims()
        {
            const string TestClaim = "TestClaim";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(TestClaim, "abc")
            }));

            var hasClaim = await sut.ExecuteAsync(
                user,
                new HasClaimPolicyContext($"BeforeRole, {TestClaim}, AfterRole"));

            Assert.True(hasClaim);
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }
}
