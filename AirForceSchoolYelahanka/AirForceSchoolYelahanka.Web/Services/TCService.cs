using AirForceSchoolYelahanka.Web.Data;
using AirForceSchoolYelahanka.Web.Services.Interfaces;
using AirForceSchoolYelahanka.Web.ViewModel.TCUpload;
using Microsoft.EntityFrameworkCore;

namespace AirForceSchoolYelahanka.Web.Services
{
    public class TCService : ITCService
    {
        private readonly AppDbContext _context;
        public TCService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<TCUploadViewModel>> GetTCsIssuedAsync()
        {
            var tcs = await _context.TCUpload.ToListAsync();

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

            return vmList;

        }
    }
}
