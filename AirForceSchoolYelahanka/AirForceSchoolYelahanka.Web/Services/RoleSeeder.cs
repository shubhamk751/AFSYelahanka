using Microsoft.AspNetCore.Identity;

namespace AirForceSchoolYelahanka.Web.Services
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            string[] roles = new[] { "Admin", "Teacher" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Admin user
            var adminEmail = "admin@school.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var user = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                if ((await userManager.CreateAsync(user, "Admin@123")).Succeeded)
                    await userManager.AddToRoleAsync(user, "Admin");
            }

            // Teacher user (optional)
            var teacherEmail = "teacher@school.com";
            var teacherUser = await userManager.FindByEmailAsync(teacherEmail);
            if (teacherUser == null)
            {
                var user = new IdentityUser
                {
                    UserName = teacherEmail,
                    Email = teacherEmail,
                    EmailConfirmed = true
                };
                if ((await userManager.CreateAsync(user, "Teacher@123")).Succeeded)
                    await userManager.AddToRoleAsync(user, "Teacher");
            }
        }
    }
}
