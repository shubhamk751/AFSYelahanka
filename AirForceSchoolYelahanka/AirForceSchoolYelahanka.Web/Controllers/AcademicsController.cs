using AirForceSchoolYelahanka.Web.Services.Interfaces;
using AirForceSchoolYelahanka.Web.ViewModel.TCUpload;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirForceSchoolYelahanka.Web.Controllers
{
    public class AcademicsController : Controller
    {
        private readonly ITCService _tcService;
        public AcademicsController(ITCService tcService)
        {
            _tcService = tcService;
        }
        [Route("/academics/junior")]
        public IActionResult Junior()
        {
            return View();
        }
        [Route("/academics/senior")]
        public IActionResult Senior()
        {
            return View();
        }
        [Route("/tcs-issued")]
        public async Task<IActionResult> TCsIssued()
        {
            var tcs = await _tcService.GetTCsIssuedAsync();

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
        [Route("/curriculum")]
        public IActionResult Curriculum()
        {
            return View();
        }
        [Route("/split-up-syllabus")]
        public IActionResult SplitUpSyllabus()
        {
            return View();
        }
    }
}
