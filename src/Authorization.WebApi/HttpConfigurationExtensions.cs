using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Authorization.WebApi
{
    public static class HttpConfigurationExtensions
    {
        public static ICollection<AbilityRequirement> ScanApiEndpointsForRequirements(this HttpConfiguration config)
        {
            var explorer = new ApiExplorer(config);

            var endpoints = new List<AbilityRequirement>();
            foreach (var description in explorer.ApiDescriptions)
            {
                var action = description.ActionDescriptor;

                var endpoint = new AbilityRequirement
                {
                    HttpMethod = description.HttpMethod.Method,
                    Controller = description.ActionDescriptor.ControllerDescriptor.ControllerName,
                    Action = description.ActionDescriptor.ActionName,
                    EndpointPath = $"/{description.RelativePath}"
                };

                var pipeline = action.GetFilterPipeline();

                foreach (var filter in pipeline.OfType<CustomAttribute>())
                {
                    endpoint.Policies.Add(filter.AbilityType.Name);
                }

                endpoints.Add(endpoint);
            }

            return endpoints;
        }
    }
}
