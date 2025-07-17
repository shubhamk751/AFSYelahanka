using AirForceSchoolYelahanka.Web.Data;
using AirForceSchoolYelahanka.Web.Models;
using AirForceSchoolYelahanka.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace AirForceSchoolYelahanka.Web.Services.Implementations
{
    public class StaffService : IStaffService
    {
        private readonly AppDbContext _context;
        public StaffService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Dictionary<string, List<Staff>>> GetStaffAsync()
        {
            var staffList = await _context.Staff
            .OrderBy(s => s.RoleCategory)
            .ThenBy(s => s.Name)
            .ToListAsync();

            return staffList
                    .GroupBy(s => string.IsNullOrWhiteSpace(s.RoleCategory) ? "Principal" : s.RoleCategory)
                    .OrderBy(g => g.Key == "Principal" ? 0 : 1) // Ensure "Principal" appears first
                    .ThenBy(g => g.Key)
                    .ToDictionary(g => g.Key, g => g.OrderBy(s => s.Name).ToList());

        }

        public async Task<Dictionary<string, int>> GetStaffStrengthAsync()
        {
            // Display roles (what you want to show)
            var displayCategories = new[] { "Principal", "PGT", "TGT", "HM", "PRT", "NTT", "Admin", "Helpers" };

            // Map DB roles to display names
            var roleMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { "Principal", "Principal" },
        { "PGT", "PGT" },
        { "TGT", "TGT" },
        { "Headmistress", "HM" }, // map Headmistress -> HM
        { "PRT", "PRT" },
        { "NTT", "NTT" },
        { "Admin", "Admin" },
        { "Helpers", "Helpers" }
    };

            var staffCounts = await _context.Staff
                .GroupBy(s => s.RoleCategory)
                .Select(g => new { Role = g.Key, Count = g.Count() })
                .ToListAsync();

            // Convert DB roles to display roles using the map
            var mappedCounts = staffCounts
                .Where(x => roleMap.ContainsKey(x.Role ?? ""))
                .GroupBy(x => roleMap[x.Role])
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Count));

            // Fill in missing categories with 0
            var result = displayCategories.ToDictionary(
                c => c,
                c => mappedCounts.ContainsKey(c) ? mappedCounts[c] : 0
            );

            return result;
        }
    }
}
