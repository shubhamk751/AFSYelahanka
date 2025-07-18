using Microsoft.AspNetCore.Mvc;

namespace AirForceSchoolYelahanka.Web.Controllers
{
    public class AboutUsController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;

        public AboutUsController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [Route("about-us")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("hierarchy")]
        public IActionResult Hierarchy()
        {
            return View();
        }
    }
}
