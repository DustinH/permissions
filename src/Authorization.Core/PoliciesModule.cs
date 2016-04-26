using Autofac;
using Assembly = System.Reflection.Assembly;
using System.Linq;

namespace Authorization
{
    public class PoliciesModule : Module
    {
        private readonly Assembly[] assemblies;

        public PoliciesModule()
        {
            assemblies = new Assembly[] { };
        }

        public PoliciesModule(params Assembly[] assemblies)
        {
            this.assemblies = assemblies;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(Policy).Assembly;

            var assembliesToRegister = assemblies.Union(new[] { assembly }).ToArray();

            builder.RegisterAssemblyTypes(assembliesToRegister)
                .Where(t => typeof(Policy).IsAssignableFrom(t))
                .As<Policy>()
                .AsSelf();

            builder.RegisterType<PoliciesService>().AsSelf();
        }
    }
}
