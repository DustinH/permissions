using System.Reflection;
using System.Security.Claims;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Owin;
using RimDev.Stuntman.Core;

namespace Authorization.MvcSample
{
    public class Startup
    {
        public static StuntmanOptions StuntmanOptions { get; } = new StuntmanOptions();

        public void Configuration(IAppBuilder app)
        {
            StuntmanOptions
             .AddUser(new StuntmanUser("user-1", "User 1")
                 .AddClaim("Awesome", "yes")
                 .AddClaim(ClaimTypes.Role, "Administrator")
                 .SetAccessToken("test-token"));

            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterModelBinders(typeof(Startup).Assembly);
            builder.RegisterModelBinderProvider();

            builder.RegisterModule<AutofacWebTypesModule>();

            builder.RegisterSource(new ViewRegistrationSource());

            builder.RegisterFilterProvider();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            app.UseStuntman(StuntmanOptions);
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
        }
    }
}
