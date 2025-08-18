using AirForceSchoolYelahanka.Web.Data;
using AirForceSchoolYelahanka.Web.Models;
using AirForceSchoolYelahanka.Web.ViewModel.TCUpload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirForceSchoolYelahanka.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TCUploadController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public TCUploadController(IWebHostEnvironment env, AppDbContext context)
        {
            _env = env;
            _context = context;
        }

        [HttpGet("/tc-upload/list")]
        public IActionResult ListTCs()
        {
            var tcs = _context.TCUpload.ToList();

            var vmList = tcs.Select(tc => new TCUploadViewModel
            {
                Id = tc.Id,
                StudentName = tc.StudentName,
                Class = tc.Class,
                Section = tc.Section,
                AdmissionNo = tc.AdmissionNo,
                IssuedOn = tc.IssuedOn,
                Remarks = tc.Remarks,
                FilePath = tc.FilePath
            }).ToList();

            return View(vmList);
        }

        [HttpGet("/tc-upload/create")]
        public IActionResult Create()
        {
            return View(new TCUploadViewModel());
        }

        [HttpPost("tc-upload/create")]
        public async Task<IActionResult> Create(TCUploadViewModel model)
        {
            if (!ModelState.IsValid || model.TCFile == null)
            {
                ModelState.AddModelError("", "Please upload the TC PDF.");
                return View(model);
            }

            // Save File
            var folder = Path.Combine(_env.WebRootPath, "assets/tc");
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.TCFile.FileName)}";
            var filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.TCFile.CopyToAsync(stream);
            }

            var entity = new TCUpload
            {
                StudentName = model.StudentName,
                AdmissionNo = model.AdmissionNo,
                Class = model.Class,
                Section = model.Section,
                IssuedOn = model.IssuedOn,
                DateOfLeaving = model.DateOfLeaving,
                Remarks = model.Remarks,
                FilePath = $"/assets/tc/{fileName}",
                UploadedBy = User.Identity?.Name ?? "admin"
            };

            _context.TCUpload.Add(entity);
            await _context.SaveChangesAsync();

            TempData["success"] = "TC uploaded successfully.";
            return RedirectToAction("ListTCs");
        }

        [HttpGet("tc-upload/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _context.TCUpload.FindAsync(id);
            if (entity == null)
                return NotFound();

            var model = new TCUploadViewModel
            {
                Id = entity.Id,
                StudentName = entity.StudentName,
                AdmissionNo = entity.AdmissionNo,
                Class = entity.Class,
                Section = entity.Section,
                IssuedOn = entity.IssuedOn,
                DateOfLeaving = entity.DateOfLeaving,
                Remarks = entity.Remarks,
                // TCFile left null (user can upload new one if needed)
            };

            return View(model);
        }

        [HttpPost("tc-upload/edit/{id}")]
        public async Task<IActionResult> Edit(int id, TCUploadViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var entity = await _context.TCUpload.FindAsync(id);
            if (entity == null)
                return NotFound();

            // Update file if new one is uploaded
            if (model.TCFile != null)
            {
                var folder = Path.Combine(_env.WebRootPath, "assets/tc");
                Directory.CreateDirectory(folder);

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.TCFile.FileName)}";
                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.TCFile.CopyToAsync(stream);
                }

                // Optionally delete old file
                if (!string.IsNullOrEmpty(entity.FilePath))
                {
                    var oldFile = Path.Combine(_env.WebRootPath, entity.FilePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFile))
                        System.IO.File.Delete(oldFile);
                }

                entity.FilePath = $"/assets/tc/{fileName}";
            }

            // Update other fields
            entity.StudentName = model.StudentName;
            entity.AdmissionNo = model.AdmissionNo;
            entity.Class = model.Class;
            entity.Section = model.Section;
            entity.IssuedOn = model.IssuedOn;
            entity.DateOfLeaving = model.DateOfLeaving;
            entity.Remarks = model.Remarks;

            _context.TCUpload.Update(entity);
            await _context.SaveChangesAsync();

            TempData["success"] = "TC updated successfully.";
            return RedirectToAction("ListTCs");
        }

        //[HttpGet("list")]
        //public IActionResult List()
        //{
        //    var data = _context.TCUpload
        //        .OrderByDescending(x => x.UploadedOn)
        //        .ToList();
        //    return View(data);
        //}
    }
}
