using Microsoft.AspNetCore.Mvc;

namespace AirForceSchoolYelahanka.Web.Controllers
{
    public class ContactUsController : Controller
    {
        [Route("contact-us")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
