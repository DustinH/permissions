using Autofac;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Authorization.Tests
{
    public class AbilitiesModuleTests
    {
        [Fact]
        public void Works()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<AbilitiesModule>();

            using (var container = builder.Build())
            {
                var policies = container.Resolve<IEnumerable<Ability>>();

                Assert.Equal(2, policies.Count());
            }
        }
    }
}
