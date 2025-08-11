using Microsoft.AspNetCore.Mvc;

namespace AirForceSchoolYelahanka.Web.Controllers
{
    public class AcademicsController : Controller
    {
        [Route("/academics/junior")]
        public IActionResult Junior()
        {
            return View();
        }
        [Route("/academics/senior")]
        public IActionResult Senior()
        {
            return View();
        }
    }
}
