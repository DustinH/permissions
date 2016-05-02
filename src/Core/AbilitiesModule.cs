using Autofac;
using Assembly = System.Reflection.Assembly;
using System.Linq;

namespace Authorization
{
    public class AbilitiesModule : Module
    {
        private readonly Assembly[] assemblies;

        public AbilitiesModule()
        {
            assemblies = new Assembly[] { };
        }

        public AbilitiesModule(params Assembly[] assemblies)
        {
            this.assemblies = assemblies;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(Ability).Assembly;

            var assembliesToRegister = assemblies.Union(new[] { assembly }).ToArray();

            builder.RegisterAssemblyTypes(assembliesToRegister)
                .Where(t => typeof(Ability).IsAssignableFrom(t))
                .As<Ability>()
                .AsSelf();

            builder.RegisterType<AbilitiesService>().AsSelf();
        }
    }
}
