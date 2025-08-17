using AirForceSchoolYelahanka.Web.Config;
using AirForceSchoolYelahanka.Web.Data;
using AirForceSchoolYelahanka.Web.Models;
using AirForceSchoolYelahanka.Web.Services.Interfaces;
using AirForceSchoolYelahanka.Web.ViewModel;
using AirForceSchoolYelahanka.Web.ViewModel.Activities;
using AirForceSchoolYelahanka.Web.ViewModel.Home;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json;

namespace AirForceSchoolYelahanka.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICmsService _cmsService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(AppDbContext context, ICmsService cmsService, ILogger<AdminController> logger)
        {
            _context = context;
            _cmsService = cmsService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login() => View();

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.AdminUsers.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, user.Role)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Invalid login");
            return View();
        }

        [Route("dashboard")]
        [HttpGet]
        public async Task<IActionResult> Index(string page = "Home")
        {
            return View();
        }

        //[HttpGet]
        //public async Task<IActionResult> Index(string page = "Home")
        //{
        //    if (!CmsPages.PageSections.ContainsKey(page))
        //    {
        //        return NotFound($"Page '{page}' not configured in CMS.");
        //    }

        //    var sectionKeys = CmsPages.PageSections[page];
        //    var model = new CMSEditViewModel
        //    {
        //        Page = page
        //    };

        //    foreach (var key in sectionKeys)
        //    {
        //        string contentJson = "[]"; // Default value
        //        try
        //        {
        //            var section = await _cmsService.GetSectionAsync(key);

        //            if (!string.IsNullOrWhiteSpace(section?.ContentJson))
        //            {
        //                bool parsedSuccessfully = false;

        //                // Try to deserialize as List<NewsItem>
        //                try
        //                {
        //                    var newsList = System.Text.Json.JsonSerializer.Deserialize<List<HomeInsertGeneric>>(section.ContentJson);
        //                    if (newsList != null)
        //                    {
        //                        contentJson = System.Text.Json.JsonSerializer.Serialize(newsList, new JsonSerializerOptions
        //                        {
        //                            WriteIndented = true
        //                        });
        //                        parsedSuccessfully = true;
        //                    }
        //                }
        //                catch { /* Try next */ }

        //                // If not List<NewsItem>, try as ActivityContentBlockViewModel
        //                if (!parsedSuccessfully)
        //                {
        //                    try
        //                    {
        //                        var activity = System.Text.Json.JsonSerializer.Deserialize<ActivityContentBlockViewModel>(section.ContentJson);
        //                        if (activity != null)
        //                        {
        //                            contentJson = System.Text.Json.JsonSerializer.Serialize(activity, new JsonSerializerOptions
        //                            {
        //                                WriteIndented = true
        //                            });
        //                            parsedSuccessfully = true;
        //                        }
        //                    }
        //                    catch { /* Fall back */ }
        //                }

        //                // If nothing worked, keep the raw ContentJson
        //                if (!parsedSuccessfully)
        //                {
        //                    _logger.LogWarning("Section '{Key}' content format could not be parsed as known types.", key);
        //                    contentJson = section.ContentJson;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, $"Error loading section '{key}'");
        //        }

        //        model.Sections.Add(new SingleCmsSectionViewModel
        //        {
        //            Key = key,
        //            ContentJson = contentJson
        //        });
        //    }

        //    return View(model);
        //}


        //[HttpPost]
        //public async Task<IActionResult> Save(CMSEditViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View("Index", model);

        //    for (int i = 0; i < model.Sections.Count; i++)
        //    {
        //        var section = model.Sections[i];
        //        try
        //        {
        //            JsonDocument.Parse(section.ContentJson); // Validate
        //            await _cmsService.UpdateSectionAsync(section.Key, section.ContentJson);
        //        }
        //        catch (System.Text.Json.JsonException)
        //        {
        //            ModelState.AddModelError($"Sections[{i}].ContentJson", $"Invalid JSON for section: {section.Key}");
        //            return View("Index", model);
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, $"Failed to update section: {section.Key}");
        //            ModelState.AddModelError($"Sections[{i}].ContentJson", $"Unexpected error saving section: {section.Key}");
        //            return View("Index", model);
        //        }
        //    }

        //    TempData["SuccessMessage"] = "All sections updated successfully.";
        //    return RedirectToAction(nameof(Index));
        //}

        [HttpGet]
        public IActionResult EditHomePageRollersSection(string sectionName = "HomePage_LatestNews")
        {
            var section = _context.CmsSections.FirstOrDefault(x => x.SectionName == sectionName);
            var items = section != null
                ? JsonConvert.DeserializeObject<List<SectionItemViewModel>>(section.ContentJson)
                : new List<SectionItemViewModel>();

            var model = new SectionContentViewModel
            {
                Id = section?.Id ?? 0,
                SelectedSectionName = sectionName,
                Items = items,
                AvailableSectionNames = new List<string>
        {
            "HomePage_LatestNews",
            "HomePage_NoticeBoard",
            "HomePage_VideoGallery"
        }
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditHomePageRollersSection(SectionContentViewModel model, string action)
        {
            if (action == "ChangeSection")
            {
                // Just reload with selected section, do not save
                return RedirectToAction("EditHomePageRollersSection", new { sectionName = model.SelectedSectionName });
            }

            // Save logic
            var section = _context.CmsSections.FirstOrDefault(x => x.SectionName == model.SelectedSectionName);
            var json = JsonConvert.SerializeObject(model.Items);

            if (section == null)
            {
                section = new CmsSection
                {
                    SectionName = model.SelectedSectionName,
                    ContentJson = json
                };
                _context.CmsSections.Add(section);
            }
            else
            {
                section.ContentJson = json;
                _context.CmsSections.Update(section);
            }

            _context.SaveChanges();

            TempData["Success"] = "Content saved successfully.";
            return RedirectToAction("EditHomePageRollersSection", new { sectionName = model.SelectedSectionName });
        }

        [HttpGet]
        public async Task<IActionResult> EditActivities(string? itemKey)
        {
            // 1. Build the list of keys from your config or from the DB
            var allKeys = CmsPages.PageSections
                               .Where(x=> x.Key != "Home")
                              .SelectMany(kv => kv.Value)
                              .Distinct()
                              .OrderBy(k => k)
                              .ToList();

            // 2. If none supplied, default to first
            itemKey ??= allKeys.FirstOrDefault();

            // 3. Load existing JSON for that key
            var section = await _cmsService.GetSectionAsync(itemKey);
            CmsContentFormViewModel model = new()
            {
                AvailableItemKeys = allKeys,
                ItemKey = itemKey,
            };

            if (section != null)
            {
                // Deserialize to your structured object
                var content = System.Text.Json.JsonSerializer.Deserialize<ActivityContentBlockViewModel>(section.ContentJson)
                              ?? new ActivityContentBlockViewModel();
                model.Title = content.Title;
                model.HtmlMainContent = content.HtmlMainContent;
                model.HtmlSidebarContent = content.HtmlSidebarContent;
                model.SidebarImageUrls = content.SidebarImageUrls;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditActivities(CmsContentFormViewModel model, string imageUrls)
        {
            // 1. Build the JSON from the form
            var content = new ActivityContentBlockViewModel
            {
                Title = model.Title,
                HtmlMainContent = model.HtmlMainContent,
                HtmlSidebarContent = model.HtmlSidebarContent,
                SidebarImageUrls = imageUrls?
                                       .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                       .Select(u => u.Trim())
                                       .ToList()
                                   ?? new List<string>()
            };

            var json = System.Text.Json.JsonSerializer.Serialize(content, new JsonSerializerOptions { WriteIndented = true });

            // 2. Save back via your service
            await _cmsService.UpdateSectionAsync(model.ItemKey, json);

            TempData["Success"] = "Content saved!";
            return RedirectToAction(nameof(EditActivities), new { itemKey = model.ItemKey });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
