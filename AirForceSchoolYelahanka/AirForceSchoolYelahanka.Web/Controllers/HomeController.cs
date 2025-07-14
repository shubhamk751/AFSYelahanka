using AirForceSchoolYelahanka.Web.Config;
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
            var pageKey = "Home"; // This can be parameterized later

            var viewModel = new Dictionary<string, Dictionary<string, string>>(); // SectionKey ? Content key-values

            try
            {
                if (!CmsPages.PageSections.TryGetValue(pageKey, out var sectionKeys))
                {
                    _logger.LogWarning($"No CMS config defined for page '{pageKey}'.");
                    return View(viewModel);
                }

                foreach (var sectionKey in sectionKeys)
                {
                    var section = await _cmsService.GetSectionAsync(sectionKey);
                    var contentDict = new Dictionary<string, string>();

                    if (!string.IsNullOrWhiteSpace(section?.ContentJson))
                    {
                        try
                        {
                            contentDict = JsonSerializer.Deserialize<Dictionary<string, string>>(section.ContentJson)
                                           ?? new Dictionary<string, string>();
                        }
                        catch (JsonException jsonEx)
                        {
                            _logger.LogError(jsonEx, $"Failed to deserialize JSON for section '{sectionKey}'.");
                        }
                    }

                    viewModel[sectionKey] = contentDict;
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading home content");
                return View(new Dictionary<string, Dictionary<string, string>>());
            }
        }

    }
}
