using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using projFront.Data;
using projFront.Repository;
using projFront.Services;
using projFront.ViewModels.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connection = builder.Configuration["ConexaoSqlite:SqliteConnectionString"];

builder.Services.AddAutoMapper(typeof(MapeamentoVMtoModel));

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<IUsuarioServices, UsuarioServices>();

builder.Services.AddScoped<IBancoServices, BancoServices>();

builder.Services.AddScoped<IBancoRepository, BancoRepository>();

builder.Services.AddScoped<IEmpresaServices, EmpresaServices>();

builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();

builder.Services.AddScoped<INotaFiscalServices, NotaFiscalServices>();

builder.Services.AddScoped<INotaFiscalRepository, NotaFiscalRepository>();

builder.Services.AddDbContext<AppDbContext>(x => 
    x.UseSqlite(connection));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
}).AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
