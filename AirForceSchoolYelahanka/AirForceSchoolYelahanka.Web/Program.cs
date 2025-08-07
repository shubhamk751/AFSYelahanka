using AirForceSchoolYelahanka.Web.Data;
using AirForceSchoolYelahanka.Web.Models;
using AirForceSchoolYelahanka.Web.Services.Implementations;
using AirForceSchoolYelahanka.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICmsService, CmsService>();
builder.Services.AddScoped<IStaffService, StaffService>();
// SQL Server DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Auth
//builder.Services.AddAuthentication("BasicAuth")
//    .AddCookie("BasicAuth", options =>
//    {
//        options.LoginPath = "/Admin/Login";
//    });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    // The default HSTS value is 30 days. To change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.AdminUsers.Any())
    {
        var adminUser = new AdminUser
        {
            Username = "AFSchoolAdmin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("P@$$w0rd@123"),
            Role = "Admin"
        };

        db.AdminUsers.Add(adminUser);
        db.SaveChanges();
    }
}

app.Run();
