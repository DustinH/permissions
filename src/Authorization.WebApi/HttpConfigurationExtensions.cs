using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Authorization.WebApi
{
    public static class HttpConfigurationExtensions
    {
        public static ICollection<PolicyRequirement> ScanEndpointsForPolicyRequirements(this HttpConfiguration config)
        {
            var explorer = new ApiExplorer(config);

            var endpoints = new List<PolicyRequirement>();
            foreach (var description in explorer.ApiDescriptions)
            {
                var action = description.ActionDescriptor;

                var endpoint = new PolicyRequirement
                {
                    HttpMethod = description.HttpMethod.Method,
                    Controller = description.ActionDescriptor.ControllerDescriptor.ControllerName,
                    Action = description.ActionDescriptor.ActionName,
                    EndpointPath = $"/{description.RelativePath}"
                };

                var pipeline = action.GetFilterPipeline();

                foreach (var filter in pipeline.OfType<PolicyAttribute>())
                {
                    endpoint.Policies.Add(filter.PolicyType.Name);
                }

                endpoints.Add(endpoint);
            }

            return endpoints;
        }
    }
}
