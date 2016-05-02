using Authorization.WebApi;
using System.Web.Http;
using Authorization.Sample.Abilities;

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
        [Ability("Awesome")]
        public IHttpActionResult ClaimPolicy()
        {
            return Ok("/claim");
        }

        [HttpGet]
        [Route("role")]
        [Role("Administrator")]
        public IHttpActionResult InRolePolicy()
        {
            return Ok("/role");
        }

        [HttpGet]
        [Route("custom")]
        [Custom(typeof(CustomAbility))]
        public IHttpActionResult CustomPolicy()
        {
            return Ok("/custom");
        }
    }
}
