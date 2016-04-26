using Authorization.Policies;
using Autofac;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Authorization.Tests.Policies
{
    public class InRolePolicyTests
    {
        private readonly IContainer container;
        private readonly InRolePolicy sut;

        public InRolePolicyTests()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<PoliciesModule>();

            container = builder.Build();

            sut = container.Resolve<InRolePolicy>();
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
                new InRolePolicyContext(TestRole));

            Assert.True(isInRole);
        }

        [Fact]
        public async Task ExecuteAsync_IsNotInRole()
        {
            const string TestRole = "TestRole";

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var isInRole = await sut.ExecuteAsync(
                user,
                new InRolePolicyContext(TestRole));

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
                new InRolePolicyContext($"BeforeRole, {TestRole}, AfterRole"));

            Assert.True(isInRole);
        }
    }
}
