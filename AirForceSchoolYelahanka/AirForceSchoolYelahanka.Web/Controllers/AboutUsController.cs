using AirForceSchoolYelahanka.Web.Services.Implementations;
using AirForceSchoolYelahanka.Web.Services.Interfaces;
using AirForceSchoolYelahanka.Web.ViewModel.AboutUs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AirForceSchoolYelahanka.Web.Controllers
{
    public class AboutUsController : Controller
    {
        private readonly IStaffService _staffService;
        private readonly ILogger<HomeController> _logger;

        public AboutUsController(IStaffService staffService, ILogger<HomeController> logger)
        {
            _staffService = staffService;
            _logger = logger;
        }
        [Route("AboutUs")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("Hierarchy")]
        public IActionResult Hierarchy()
        {
            return View();
        }
        [Route("Team-&-Visiting-Hours")]
        public async Task<IActionResult> Directory()
        {
            try
            {
                var groupedStaff = await _staffService.GetStaffAsync();
                var countPerRole = await _staffService.GetStaffStrengthAsync();

                var viewModel = new StaffDirectoryViewModel
                {
                    StaffGroupedByRole = groupedStaff,
                    StaffCountByRole = countPerRole
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("Timings")]
        public IActionResult Timings()
        {
            return View();
        }
    }
}
