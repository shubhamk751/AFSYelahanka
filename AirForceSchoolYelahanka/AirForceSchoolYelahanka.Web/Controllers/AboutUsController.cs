using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [Route("code-of-conduct")]
        public IActionResult CodeOfConduct()
        {
            return View();
        }
        [Route("admission-guidelines")]
        public IActionResult AdmissionGuidelines()
        {
            return View();
        }
        [Route("leave-absence-rule")]
        public IActionResult LeaveAbsenceRule()
        {
            return View();
        }
    }
}
