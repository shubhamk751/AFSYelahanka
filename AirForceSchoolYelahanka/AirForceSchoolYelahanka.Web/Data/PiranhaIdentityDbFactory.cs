using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AirForceSchoolYelahanka.Web.Data
{
    public class PiranhaIdentityDbFactory : IDesignTimeDbContextFactory<PiranhaIdentityDb>
    {
        public PiranhaIdentityDb CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<PiranhaIdentityDb>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            return new PiranhaIdentityDb(optionsBuilder.Options);
        }
    }
}
