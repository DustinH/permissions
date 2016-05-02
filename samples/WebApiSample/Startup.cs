using Autofac;
using Autofac.Integration.WebApi;
using Owin;
using RimDev.Stuntman.Core;
using System.Security.Claims;
using System.Web.Http;

namespace Authorization.Sample
{
    public class Startup
    {
        public static readonly StuntmanOptions StuntmanOptions = new StuntmanOptions();

        public void Configuration(IAppBuilder app)
        {
            StuntmanOptions
              .AddUser(new StuntmanUser("user-1", "User 1")
                  .AddClaim("Awesome", "yes")
                  .AddClaim(ClaimTypes.Role, "Administrator")
                  .SetAccessToken("test-token"));

            var httpConfiguration = new HttpConfiguration();

            httpConfiguration.MapHttpAttributeRoutes();

            var builder = new ContainerBuilder();

            builder.RegisterModule(new WebApiModule(httpConfiguration));
            builder.RegisterModule(new AbilitiesModule(typeof(Startup).Assembly));

            var container = builder.Build();

            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseStuntman(StuntmanOptions);

            app.UseAutofacMiddleware(container)
               .UseAutofacWebApi(httpConfiguration)
               .UseWebApi(httpConfiguration);
        }
    }
}
