using AirForceSchoolYelahanka.Web.Models;
using System.Threading.Tasks;

namespace AirForceSchoolYelahanka.Web.Services.Interfaces
{
    public interface IStaffService
    {
        Task<Dictionary<string, List<Staff>>> GetStaffAsync();
        Task<Dictionary<string, int>> GetStaffStrengthAsync();
    }
}
