using AirForceSchoolYelahanka.Web.Models;
using AirForceSchoolYelahanka.Web.Services.Interfaces;
using AirForceSchoolYelahanka.Web.ViewModel.Activities;
using AirForceSchoolYelahanka.Web.ViewModel.Home;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.Json;
using static System.Collections.Specialized.BitVector32;

namespace AirForceSchoolYelahanka.Web.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ICmsService _cmsService;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        public ActivitiesController(ICmsService cmsService, ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _cmsService = cmsService;
            _logger = logger;
            _env = env;
        }

        [Route("cca-junior")]
        public IActionResult CCAJunior()
        {
            return View();
        }

        [Route("cca-senior")]
        public IActionResult CCASenior()
        {
            return View();
        }

        [Route("activity")]
        public async Task<IActionResult> Activity( string type, string division, string slug)
        {
            string sectionKey = $"{type}.{division}.{slug}";

            var section = await _cmsService.GetSectionAsync(sectionKey);
            if (section == null)
                return NotFound();

            var content = JsonConvert.DeserializeObject<ActivityContentBlockViewModel>(section.ContentJson);
            if (content == null)
                return NotFound();

            var folderPath = Path.Combine(_env.WebRootPath, "assets", "images", "activities", type, division, slug);
            var imageUrls = Directory.Exists(folderPath)
                ? Directory.GetFiles(folderPath)
                    .Where(f => f.EndsWith(".jpg") || f.EndsWith(".jpeg") || f.EndsWith(".png"))
                    .Select(f => $"/assets/images/activities/{type}/{division}/{slug}/{Path.GetFileName(f)}")
                    .ToList()
                : new List<string>();
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            var model = new ActivityContentBlockViewModel
            {
                Division = textInfo.ToTitleCase(division.ToLower()),
                Type = textInfo.ToTitleCase(type.ToLower()),
                Slug = textInfo.ToTitleCase(slug.ToLower()),
                Title = content.Title,
                HtmlMainContent = content.HtmlMainContent,
                HtmlSidebarContent = content.HtmlSidebarContent,
                SidebarImageUrls = imageUrls
            };

            return View(model);
        }
    }
}
