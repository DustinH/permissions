using System.Reflection;
using System.Web.Mvc;
using Authorization.Mvc;
using Authorization.MvcSample.Abilities;

namespace Authorization.MvcSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var actions = AbilityScanner.ScanMvcEndpointsForRequirements(Assembly.GetExecutingAssembly());

            return Json(actions, JsonRequestBehavior.AllowGet);
        }

        [Authenticate]
        [Custom(typeof(CustomAbility))]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
