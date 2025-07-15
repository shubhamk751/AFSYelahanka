using AirForceSchoolYelahanka.Web.Config;
using AirForceSchoolYelahanka.Web.Models;
using AirForceSchoolYelahanka.Web.Services.Interfaces;
using AirForceSchoolYelahanka.Web.ViewModel;
using AirForceSchoolYelahanka.Web.ViewModel.Home;
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
            const string pageKey = "Home";

            var viewModel = new HomePageViewModel();

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
                    if (section == null || string.IsNullOrWhiteSpace(section.ContentJson))
                        continue;

                    try
                    {
                        switch (sectionKey)
                        {
                            case "HomePage_LatestNews":
                                viewModel.LatestNews = JsonSerializer.Deserialize<List<NewsItem>>(section.ContentJson) ?? new();
                                break;

                            case "HomePage_NoticeBoard":
                                viewModel.NoticeBoard = JsonSerializer.Deserialize<List<NoticeItem>>(section.ContentJson) ?? new();
                                break;

                            case "HomePage_RecentActivity":
                                viewModel.RecentActivities = JsonSerializer.Deserialize<List<ActivityItem>>(section.ContentJson) ?? new();
                                break;

                            default:
                                // Store generic section content (e.g. title, subtitle) as dictionary
                                var sectionContent = JsonSerializer.Deserialize<Dictionary<string, string>>(section.ContentJson) ?? new();
                                viewModel.SectionContent[sectionKey] = sectionContent;
                                break;
                        }
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError(ex, $"Failed to deserialize content for section '{sectionKey}'.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error loading home page content.");
            }

            return View(viewModel);
        }
    }
}
