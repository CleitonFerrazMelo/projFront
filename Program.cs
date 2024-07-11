using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using projFront.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connection = builder.Configuration["ConexaoSqlite:SqliteConnectionString"];

builder.Services.AddDbContext<AppDbContext>(x => 
    x.UseSqlite(connection));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
}).AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
