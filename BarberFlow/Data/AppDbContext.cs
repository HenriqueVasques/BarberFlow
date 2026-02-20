using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberFlow.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions dbContextOptions ) : base(dbContextOptions) { }

        DbSet<Usuario> Usuarios { get; set; }
        DbSet<Empresa>  Empresas { get; set; }

    }
}
