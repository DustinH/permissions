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
            var endpoints = Configuration.ScanApiEndpointsForRequirements();

            return Json(endpoints);
        }
    }
}
