using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using projFront.Data;
using projFront.Models;
using projFront.Repository;
using projFront.Services;
using projFront.ViewModels.Mappings;
using Rotativa.AspNetCore;

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

builder.Services.AddDefaultIdentity<ApplicationUser>().AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://intranet.facchinieventos.com.br")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowSpecificOrigin");


app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseRotativa();

app.MapRazorPages();

app.Run();
