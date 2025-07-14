using AirForceSchoolYelahanka.Web.Data;
using AirForceSchoolYelahanka.Web.Services.Implementations;
using AirForceSchoolYelahanka.Web.Services.Interfaces;
using AirForceSchoolYelahanka.Web.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace AirForceSchoolYelahanka.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICmsService _cmsService;

        public AdminController(AppDbContext context, ICmsService cmsService)
        {
            _context = context;
            _cmsService = cmsService;
        }

        // GET: /Admin/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Admin/Login
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

        // GET: /Admin
        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{
        //    var section = await _cmsService.GetSectionAsync("HomePageSection1");

        //    string contentJson = "{}";

        //    if (!string.IsNullOrWhiteSpace(section?.ContentJson))
        //    {
        //        try
        //        {
        //            // Optional: Deserialize and re-serialize to pretty-print (if needed)
        //            var contentDict = JsonSerializer.Deserialize<Dictionary<string, string>>(section.ContentJson)
        //                              ?? new Dictionary<string, string>();

        //            contentJson = JsonSerializer.Serialize(contentDict, new JsonSerializerOptions
        //            {
        //                WriteIndented = true
        //            });
        //        }
        //        catch (JsonException)
        //        {
        //            // Log or handle corrupted content
        //            contentJson = section.ContentJson; // Keep as-is even if invalid
        //        }
        //    }

        //    var model = new CMSEditViewModel
        //    {
        //        Key = "HomePageSection1",
        //        ContentJson = contentJson
        //    };

        //    return View(model);
        //}
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sectionKeys = new[] { "HomePageSection1", "HomePageSection2", "FooterContent" }; // Add more keys as needed

            var model = new CMSEditViewModel();

            foreach (var key in sectionKeys)
            {
                var section = await _cmsService.GetSectionAsync(key);
                string contentJson = "{}";

                if (!string.IsNullOrWhiteSpace(section?.ContentJson))
                {
                    try
                    {
                        var contentDict = JsonSerializer.Deserialize<Dictionary<string, string>>(section.ContentJson)
                                          ?? new Dictionary<string, string>();

                        contentJson = JsonSerializer.Serialize(contentDict, new JsonSerializerOptions
                        {
                            WriteIndented = true
                        });
                    }
                    catch (JsonException)
                    {
                        contentJson = section.ContentJson; // retain as-is
                    }
                }

                model.Sections.Add(new SingleCmsSectionViewModel
                {
                    Key = key,
                    ContentJson = contentJson
                });
            }

            return View(model);
        }


        //[Authorize]
        // POST: /Admin/Save
        //[HttpPost]
        //public async Task<IActionResult> Save(CMSEditViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View("Index", model);

        //    try
        //    {
        //        // Validate JSON
        //        JsonDocument.Parse(model.ContentJson);
        //    }
        //    catch (JsonException)
        //    {
        //        ModelState.AddModelError("ContentJson", "Invalid JSON format");
        //        return View("Index", model);
        //    }

        //    await _cmsService.UpdateSectionAsync(model.Key, model.ContentJson);
        //    return RedirectToAction(nameof(Index));
        //}
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Save(CMSEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            foreach (var section in model.Sections)
            {
                try
                {
                    JsonDocument.Parse(section.ContentJson); // Validate
                    await _cmsService.UpdateSectionAsync(section.Key, section.ContentJson);
                }
                catch (JsonException)
                {
                    ModelState.AddModelError($"Sections[{model.Sections.IndexOf(section)}].ContentJson", $"Invalid JSON for section: {section.Key}");
                    return View("Index", model);
                }
            }

            TempData["SuccessMessage"] = "All sections updated successfully.";
            return RedirectToAction(nameof(Index));
        }


        // GET: /Admin/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
