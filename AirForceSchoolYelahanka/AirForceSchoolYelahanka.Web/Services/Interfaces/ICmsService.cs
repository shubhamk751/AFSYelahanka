using AirForceSchoolYelahanka.Web.Models;

namespace AirForceSchoolYelahanka.Web.Services.Interfaces
{
    public interface ICmsService
    {
        Task<CmsSection?> GetSectionAsync(string sectionName);
        Task UpdateSectionAsync(string sectionName, string jsonContent);
    }
}
