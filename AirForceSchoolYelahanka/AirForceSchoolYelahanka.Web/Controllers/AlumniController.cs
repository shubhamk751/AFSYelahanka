using Microsoft.AspNetCore.Mvc;

namespace AirForceSchoolYelahanka.Web.Controllers
{
    public class AlumniController : Controller
    {
        [Route("/alumni")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
