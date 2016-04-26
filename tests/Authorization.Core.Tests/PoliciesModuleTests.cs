using Autofac;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Authorization.Tests
{
    public class PoliciesModuleTests
    {
        [Fact]
        public void Works()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<PoliciesModule>();

            using (var container = builder.Build())
            {
                var policies = container.Resolve<IEnumerable<Policy>>();

                Assert.Equal(3, policies.Count());
            }
        }
    }
}
