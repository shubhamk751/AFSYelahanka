using AirForceSchoolYelahanka.Web.Models;
using AirForceSchoolYelahanka.Web.Services.Interfaces;
using AirForceSchoolYelahanka.Web.ViewModel.Activities;
using AirForceSchoolYelahanka.Web.ViewModel.Home;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        [Route("cca-junior-plantation")]
        public IActionResult CCAJuniorPlantation()
        {
            return View();
        }
        [Route("cca-junior-diwali")]
        public IActionResult CCAJuniorDiwali()
        {
            return View();
        }
        [Route("cca-junior-othercca")]
        public IActionResult CCAJuniorOtherCCA()
        {
            return View();
        }
        [Route("activity")]
        public async Task<IActionResult> Activity(string slug)
        {
            string sectionKey = $"CCA.Junior.{slug}";

            var section = await _cmsService.GetSectionAsync(sectionKey);
            if (section == null)
                return NotFound();

            var content = JsonConvert.DeserializeObject<ActivityContentBlockViewModel>(section.ContentJson);
            if (content == null)
                return NotFound();

            var folderPath = Path.Combine(_env.WebRootPath, "images", "activities", "cca", "junior", slug);
            var imageUrls = Directory.Exists(folderPath)
                ? Directory.GetFiles(folderPath)
                    .Where(f => f.EndsWith(".jpg") || f.EndsWith(".jpeg") || f.EndsWith(".png"))
                    .Select(f => $"/images/activities/cca/junior/{slug}/{Path.GetFileName(f)}")
                    .ToList()
                : new List<string>();

            var model = new ActivityContentBlockViewModel
            {
                Title = content.Title,
                HtmlMainContent = content.HtmlMainContent,
                HtmlSidebarContent = content.HtmlSidebarContent,
                SidebarImageUrls = imageUrls
            };

            return View(model);
        }
    }
}
