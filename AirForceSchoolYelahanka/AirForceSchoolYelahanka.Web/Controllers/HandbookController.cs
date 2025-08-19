using AirForceSchoolYelahanka.Web.Services.Interfaces;
using AirForceSchoolYelahanka.Web.ViewModel.Handbook;
using Microsoft.AspNetCore.Mvc;

namespace AirForceSchoolYelahanka.Web.Controllers
{
    public class HandbookController : Controller
    {
        private readonly IStaffService _staffService;
        private readonly ILogger<HandbookController> _logger;
        public HandbookController(IStaffService staffService, ILogger<HandbookController> logger)
        {
            _staffService = staffService;
            _logger = logger;
        }
        [Route("team-&-visiting-hours")]
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

        [Route("timings")]
        public IActionResult Timings()
        {
            return View();
        }
        [Route("uniform")]
        public IActionResult Uniform()
        {
            return View();
        }

        [Route("fees")]
        public IActionResult FeeStructure()
        {
            return View();
        }

        [Route("book-list")]
        public IActionResult BookList()
        {
            return View();
        }
        [Route("code-of-conduct")]
        public IActionResult CodeOfConduct()
        {
            return View();
        }
    }
}
