using AirForceSchoolYelahanka.Web.ViewModel.AdminAsset;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirForceSchoolYelahanka.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminAssetController : Controller
    {
        private readonly string _rootFolder = "wwwroot/assets";

        [HttpGet]
        public IActionResult Index(string path = "")
        {
            string fullPath = Path.Combine(_rootFolder, path ?? "");
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            var items = new List<AssetItem>();

            foreach (var dir in Directory.GetDirectories(fullPath))
            {
                items.Add(new AssetItem
                {
                    Name = Path.GetFileName(dir),
                    IsFolder = true,
                    Path = Path.Combine(path ?? "", Path.GetFileName(dir)).Replace("\\", "/")
                });
            }

            foreach (var file in Directory.GetFiles(fullPath))
            {
                items.Add(new AssetItem
                {
                    Name = Path.GetFileName(file),
                    IsFolder = false,
                    Path = Path.Combine(path ?? "", Path.GetFileName(file)).Replace("\\", "/")
                });
            }

            ViewBag.CurrentPath = path ?? "";
            ViewBag.Breadcrumbs = GetBreadcrumbs(path);
            return View(items);
        }

        private List<(string Path, string Name)> GetBreadcrumbs(string path)
        {
            var breadcrumbs = new List<(string Path, string Name)>
            {
                ("", "Root")
            };

            if (!string.IsNullOrEmpty(path))
            {
                var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
                var accumulatedPath = "";

                foreach (var segment in segments)
                {
                    accumulatedPath = string.IsNullOrEmpty(accumulatedPath)
                        ? segment
                        : $"{accumulatedPath}/{segment}";

                    breadcrumbs.Add((accumulatedPath, segment));
                }
            }

            return breadcrumbs;
        }

        [HttpPost]
        public IActionResult CreateFolder(string currentPath, string folderName)
        {
            var newFolderPath = Path.Combine(_rootFolder, currentPath ?? "", folderName);
            if (!Directory.Exists(newFolderPath))
                Directory.CreateDirectory(newFolderPath);

            return RedirectToAction("Index", new { path = currentPath });
        }

        [HttpPost]
        public async Task<IActionResult> Upload(string currentPath, IFormFile file)
        {
            if (file != null)
            {
                var folderPath = Path.Combine(_rootFolder, currentPath ?? "");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, file.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
            }

            return RedirectToAction("Index", new { path = currentPath });
        }

        [HttpPost]
        public IActionResult Delete(string currentPath, string itemPath, bool isFolder)
        {
            string fullPath = Path.Combine(_rootFolder, itemPath);

            if (isFolder && Directory.Exists(fullPath))
                Directory.Delete(fullPath, true);
            else if (!isFolder && System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

            return RedirectToAction("Index", new { path = currentPath });
        }
    }
}
