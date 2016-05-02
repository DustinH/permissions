using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Authorization.Abilities;
using Autofac;
using Xunit;

namespace Authorization.Tests.Abilities
{
    public class HasClaimAbilityTests : IDisposable
    {
        private readonly IContainer container;
        private readonly HasClaimAbility sut;

        public HasClaimAbilityTests()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<AbilitiesModule>();

            container = builder.Build();

            sut = container.Resolve<HasClaimAbility>();
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
                new HasClaimAbilityContext(TestClaim));

            Assert.True(hasClaim);
        }

        [Fact]
        public async Task ExecuteAsync_DoesNotHaveClaim()
        {
            const string TestClaim = "TestClaim";

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var hasClaim = await sut.ExecuteAsync(
                user,
                new HasClaimAbilityContext(TestClaim));

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
                new HasClaimAbilityContext($"BeforeRole, {TestClaim}, AfterRole"));

            Assert.True(hasClaim);
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }
}
