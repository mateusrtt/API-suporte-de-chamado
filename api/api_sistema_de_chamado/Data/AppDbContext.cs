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
        public DbSet<ChamadoModel> Chamado { get; set; }
        public DbSet<AcessoLogModel> AcessoLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChamadoModel>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Chamados)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AcessoLogModel>()
               .HasOne<UsuariosModel>()
               .WithMany()
               .HasForeignKey(l => l.UsuarioId)
               .OnDelete(DeleteBehavior.SetNull);
        }
    }
}