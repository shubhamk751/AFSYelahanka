using Microsoft.AspNetCore.Mvc;

namespace AirForceSchoolYelahanka.Web.Controllers
{
    public class InfrastructureController : Controller
    {
        [Route("junior-wing")]
        public IActionResult JuniorWing()
        {
            return View();
        }
        [Route("senior-wing")]
        public IActionResult SeniorWing()
        {
            return View();
        }
    }
}
