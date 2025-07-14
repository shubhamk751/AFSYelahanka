using AirForceSchoolYelahanka.Web.Data;
using AirForceSchoolYelahanka.Web.Models;
using AirForceSchoolYelahanka.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirForceSchoolYelahanka.Web.Services.Implementations
{
    public class CmsService : ICmsService
    {
        private readonly AppDbContext _context;

        public CmsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CmsSection?> GetSectionAsync(string sectionName)
        {
            return await _context.CmsSections.FirstOrDefaultAsync(s => s.SectionName == sectionName);
        }

        public async Task UpdateSectionAsync(string sectionName, string jsonContent)
        {
            var section = await GetSectionAsync(sectionName);
            if (section is not null)
            {
                section.ContentJson = jsonContent;
            }
            else
            {
                section = new CmsSection
                {
                    SectionName = sectionName,
                    ContentJson = jsonContent
                };
                _context.CmsSections.Add(section);
            }

            await _context.SaveChangesAsync();
        }

    }
}
