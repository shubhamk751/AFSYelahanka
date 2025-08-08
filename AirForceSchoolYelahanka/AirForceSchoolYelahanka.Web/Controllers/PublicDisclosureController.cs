using Microsoft.AspNetCore.Mvc;

namespace AirForceSchoolYelahanka.Web.Controllers
{
    public class PublicDisclosureController : Controller
    {
        [Route("mandatory-public-disclosure")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
