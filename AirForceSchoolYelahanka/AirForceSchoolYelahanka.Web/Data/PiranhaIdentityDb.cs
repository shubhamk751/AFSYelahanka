using Microsoft.EntityFrameworkCore;
using Piranha.AspNetCore.Identity;
using Piranha.Data.EF.SQLServer;

namespace AirForceSchoolYelahanka.Web.Data
{
    public class PiranhaIdentityDb : Db<PiranhaIdentityDb>
    {
        public PiranhaIdentityDb(DbContextOptions<PiranhaIdentityDb> options)
            : base(options)
        {
        }
    }
}
