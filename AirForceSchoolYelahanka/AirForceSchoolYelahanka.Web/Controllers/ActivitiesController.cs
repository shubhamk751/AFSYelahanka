using AirForceSchoolYelahanka.Web.Services.Interfaces;
using AirForceSchoolYelahanka.Web.ViewModel.Activities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;

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

        //saving this if seperate view is required
        //[Route("/cca")]
        //public IActionResult CCA()
        //{
        //    return View();
        //}

        //[Route("celebrations")]
        //public IActionResult Celebrations()
        //{
        //    return View();
        //}

        [Route("activity-lp")]
        public IActionResult ActivityLP(string pageSlug)
        {
            ViewData["PageSlug"] = pageSlug;
            return View();
        }


        [Route("activity")]
        public async Task<IActionResult> Activity( string type, string slug)
        {
            string sectionKey = $"{type}.{slug}";

            var section = await _cmsService.GetSectionAsync(sectionKey);
            if (section == null)
                return NotFound();

            var content = JsonConvert.DeserializeObject<ActivityContentBlockViewModel>(section.ContentJson);
            if (content == null)
                return NotFound();

            var folderPath = Path.Combine(_env.WebRootPath, "assets", "images", "activities", type,  slug);
            var imageUrls = Directory.Exists(folderPath)
                ? Directory.GetFiles(folderPath)
                    .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    .Select(f => $"/assets/images/activities/{type}/{slug}/{Path.GetFileName(f)}")
                    .ToList()
                : new List<string>();
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            string[] keywords = { "CCA", "NCC" };
            var model = new ActivityContentBlockViewModel
            {
                Type = keywords.Any(k => type.Contains(k, StringComparison.OrdinalIgnoreCase))
                            ? type.ToUpper()
                            : textInfo.ToTitleCase(type.ToLower()),
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
