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
    }
}
