using System.Security.Claims;
using System.Threading.Tasks;
using Authorization.Abilities;
using Autofac;
using Xunit;

namespace Authorization.Tests.Abilities
{
    public class InRoleAbilityTests
    {
        private readonly IContainer container;
        private readonly InRoleAbility sut;

        public InRoleAbilityTests()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<AbilitiesModule>();

            container = builder.Build();

            sut = container.Resolve<InRoleAbility>();
        }

        [Fact]
        public async Task ExecuteAsync_DoesNotThrowForNullArgs()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            await sut.ExecuteAsync(user);
        }

        [Fact]
        public async Task ExecuteAsync_IsInRole()
        {
            const string TestRole = "TestRole";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, TestRole)
            }));

            var isInRole = await sut.ExecuteAsync(
                user,
                new InRoleAbilityContext(TestRole));

            Assert.True(isInRole);
        }

        [Fact]
        public async Task ExecuteAsync_IsNotInRole()
        {
            const string TestRole = "TestRole";

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var isInRole = await sut.ExecuteAsync(
                user,
                new InRoleAbilityContext(TestRole));

            Assert.False(isInRole);
        }

        [Fact]
        public async Task ExecuteAsync_IsInMultipleRoles()
        {
            const string TestRole = "TestRole";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, TestRole)
            }));

            var isInRole = await sut.ExecuteAsync(
                user,
                new InRoleAbilityContext($"BeforeRole, {TestRole}, AfterRole"));

            Assert.True(isInRole);
        }
    }
}
