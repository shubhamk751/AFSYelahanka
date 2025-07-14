using AirForceSchoolYelahanka.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AirForceSchoolYelahanka.Web.Data
{
    public class Seed
    {
        public static void Initialize(AppDbContext context)
        {
            if (!context.AdminUsers.Any())
            {
                var password = "admin123";
                var hasher = new PasswordHasher<AdminUser>();
                var user = new AdminUser { Username = "admin" };
                user.PasswordHash = hasher.HashPassword(user, password);

                context.AdminUsers.Add(user);
                context.SaveChanges();
            }

            if (!context.CmsSections.Any())
            {
                context.CmsSections.Add(new CmsSection
                {
                    SectionName = "Home_Highlight",
                    ContentJson = "{\"heading\": \"Welcome to Air Force School\", \"description\": \"Empowering young minds...\"}"
                });

                context.SaveChanges();
            }
        }
    }
}
