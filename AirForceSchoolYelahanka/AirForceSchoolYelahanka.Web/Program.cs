using AirForceSchoolYelahanka.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Piranha;
using Piranha.AspNetCore.Identity;
using Piranha.Data.EF.SQLServer;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddPiranha(options =>
{
    options.UseCms();
    options.UseManager();
    options.UseTinyMCE();
    options.UseFileStorage();
    options.UseEF<SQLServerDb>(db =>
        db.UseSqlServer(connectionString));
});

// ?? Wire up identity db using YOUR class
builder.Services.AddPiranhaIdentityWithSeed<PiranhaIdentityDb>(
    db => db.UseSqlServer(connectionString),
    identity =>
    {
        identity.SignIn.RequireConfirmedAccount = false;
    },
    cookie =>
    {
        cookie.LoginPath = "/login";
        cookie.AccessDeniedPath = "/forbidden";
    });

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UsePiranha(cms =>
{
    cms.UseManager();
    cms.UseTinyMCE();
});

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PiranhaIdentityDb>();
    db.Database.Migrate(); 
}
app.Run();
