using AirForceSchoolYelahanka.Web.Models;
using AirForceSchoolYelahanka.Web.Services;
using AirForceSchoolYelahanka.Web.Services.Implementations;
using AirForceSchoolYelahanka.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AirForceSchoolYelahanka.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICmsService _cmsService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ICmsService cmsService, ILogger<HomeController> logger)
        {
            _cmsService = cmsService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var section = await _cmsService.GetSectionAsync("HomePageSection1");
                var model = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(section?.ContentJson))
                {
                    model = JsonSerializer.Deserialize<Dictionary<string, string>>(section.ContentJson)
                             ?? new Dictionary<string, string>();
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading home content");
                return View(new Dictionary<string, string>());
            }
        }

    }
}
