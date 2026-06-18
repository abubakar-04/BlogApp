// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using BlogApp.Data;
// using BlogApp.Models;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.Extensions.Options;
// var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddDbContext<BlogAppContext>(options =>
//     options.UseSqlite(builder.Configuration.GetConnectionString("BlogAppContext") ?? throw new InvalidOperationException("Connection string 'BlogAppContext' not found.")));
// builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<BlogAppContext>();

// // Add services to the container.
// builder.Services.AddControllersWithViews();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }

// // app.UseHttpsRedirection();
// app.UseRouting();
// app.UseAuthentication();
// app.UseAuthorization();



// // app.MapStaticAssets();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}")
//     .WithStaticAssets();
// app.MapRazorPages();

// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<BlogAppContext>();
//     db.Database.Migrate();
// }

// app.Run();

using Microsoft.EntityFrameworkCore;
using BlogApp.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlogAppContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("BlogAppContext")
        ?? throw new InvalidOperationException("Connection string 'BlogAppContext' not found.")
    ));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<BlogAppContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ❌ removed UseHttpsRedirection for Render compatibility

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 🔥 AUTO MIGRATION (IMPORTANT)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BlogAppContext>();
    db.Database.Migrate();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
