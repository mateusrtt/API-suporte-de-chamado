using api_sistema_de_chamado.Models;
using Microsoft.EntityFrameworkCore;

namespace api_sistema_de_chamado.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<UsuariosModel> Usuario { get; set; }
    }
}
