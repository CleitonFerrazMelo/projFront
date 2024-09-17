using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using projFront.Models;

namespace projFront.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Banco> Bancos { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<NotaFiscal> NotaFiscal { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        

    }
}