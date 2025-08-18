using AirForceSchoolYelahanka.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AirForceSchoolYelahanka.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CmsSection> CmsSections => Set<CmsSection>();
        public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
        public DbSet<Staff> Staff => Set<Staff>();
        public DbSet<TCUpload> TCUpload => Set<TCUpload>();
    }
}
