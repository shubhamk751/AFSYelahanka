using AirForceSchoolYelahanka.Web.Config;
using AirForceSchoolYelahanka.Web.Data;
using AirForceSchoolYelahanka.Web.Services.Interfaces;
using AirForceSchoolYelahanka.Web.ViewModel;
using AirForceSchoolYelahanka.Web.ViewModel.Home;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;

namespace AirForceSchoolYelahanka.Web.Controllers
{
    [Authorize]
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
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Invalid login");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string page = "Home")
        {
            if (!CmsPages.PageSections.ContainsKey(page))
            {
                return NotFound($"Page '{page}' not configured in CMS.");
            }

            var sectionKeys = CmsPages.PageSections[page];
            var model = new CMSEditViewModel
            {
                Page = page
            };

            foreach (var key in sectionKeys)
            {
                string contentJson = "[]"; // Default as array

                try
                {
                    var section = await _cmsService.GetSectionAsync(key);
                    if (!string.IsNullOrWhiteSpace(section?.ContentJson))
                    {
                        // Deserialize to validate it matches the expected List<NewsItem> format
                        var parsed = JsonSerializer.Deserialize<List<NewsItem>>(section.ContentJson);

                        // If successful, pretty-print it
                        contentJson = JsonSerializer.Serialize(parsed, new JsonSerializerOptions
                        {
                            WriteIndented = true
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error loading section {key}");
                }

                model.Sections.Add(new SingleCmsSectionViewModel
                {
                    Key = key,
                    ContentJson = contentJson
                });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(CMSEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            for (int i = 0; i < model.Sections.Count; i++)
            {
                var section = model.Sections[i];
                try
                {
                    JsonDocument.Parse(section.ContentJson); // Validate
                    await _cmsService.UpdateSectionAsync(section.Key, section.ContentJson);
                }
                catch (JsonException)
                {
                    ModelState.AddModelError($"Sections[{i}].ContentJson", $"Invalid JSON for section: {section.Key}");
                    return View("Index", model);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to update section: {section.Key}");
                    ModelState.AddModelError($"Sections[{i}].ContentJson", $"Unexpected error saving section: {section.Key}");
                    return View("Index", model);
                }
            }

            TempData["SuccessMessage"] = "All sections updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
