using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Authorization.WebApi;

namespace Authorization.Sample.Controllers
{
    public class DocsController : ApiController
    {
        [HttpGet]
        [Route("docs")]
        public IHttpActionResult Get()
        {
            var explorer = new ApiExplorer(Configuration);

            var endpoints = new List<EndpointSummary>();
            foreach (var description in explorer.ApiDescriptions)
            {
                var action = description.ActionDescriptor;

                var endpoint = new EndpointSummary
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

            return Json(endpoints);
        }

        private class EndpointSummary
        {
            public EndpointSummary()
            {
                Policies = new HashSet<string>();
            }

            public string HttpMethod { get; set; }

            public string Controller { get; set; }

            public string Action { get; set; }

            public string EndpointPath { get; set; }

            public ICollection<string> Policies { get; }
        }
    }
}
