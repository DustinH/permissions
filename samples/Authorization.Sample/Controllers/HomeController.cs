using Authorization.Sample.Policies;
using Authorization.WebApi;
using System.Web.Http;

namespace Authorization.Sample.Controllers
{
    [Authenticate]
    public class HomeController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("")]
        public IHttpActionResult Root()
        {
            return Ok("Welcome to the root path.");
        }

        [HttpGet]
        [Route("claim")]
        [ClaimPolicy(Claim = "Awesome")]
        public IHttpActionResult ClaimPolicy()
        {
            return Ok("/claim");
        }

        [HttpGet]
        [Route("role")]
        [InRolePolicy(Roles = "Administrator")]
        public IHttpActionResult InRolePolicy()
        {
            return Ok("/role");
        }

        [HttpGet]
        [Route("custom")]
        [Policy(typeof(CustomPolicy))]
        public IHttpActionResult CustomPolicy()
        {
            return Ok("/custom");
        }
    }
}
