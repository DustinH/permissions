using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Authorization.Sample
{
    internal class WebApiModule : Module
    {
        private readonly HttpConfiguration config;

        public WebApiModule(HttpConfiguration httpConfiguration)
        {
            if (httpConfiguration == null)
                throw new ArgumentNullException(nameof(httpConfiguration));

            config = httpConfiguration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(config)
                .ExternallyOwned();

            builder.RegisterApiControllers(ThisAssembly);

            builder.RegisterHttpRequestMessage(config);

            builder.RegisterWebApiModelBinderProvider();
            builder.RegisterWebApiModelBinders(ThisAssembly);

            builder.Register(c => new UrlHelper(c.Resolve<HttpRequestMessage>()))
                .InstancePerLifetimeScope()
                .As<UrlHelper>();
        }
    }
}
