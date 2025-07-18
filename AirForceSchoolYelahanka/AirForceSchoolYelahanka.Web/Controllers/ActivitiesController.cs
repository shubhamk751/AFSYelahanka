using Microsoft.AspNetCore.Mvc;

namespace AirForceSchoolYelahanka.Web.Controllers
{
    public class ActivitiesController : Controller
    {
        [Route("cca-junior")]
        public IActionResult CCAJunior()
        {
            return View();
        }
        [Route("cca-junior-plantation")]
        public IActionResult CCAJuniorPlantation()
        {
            return View();
        }
        [Route("cca-junior-diwali")]
        public IActionResult CCAJuniorDiwali()
        {
            return View();
        }
        [Route("cca-junior-othercca")]
        public IActionResult CCAJuniorOtherCCA()
        {
            return View();
        }
    }
}
