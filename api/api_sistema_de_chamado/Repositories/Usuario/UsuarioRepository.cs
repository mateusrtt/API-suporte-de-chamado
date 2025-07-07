using api_sistema_de_chamado.Data;
using api_sistema_de_chamado.Models;
using Microsoft.EntityFrameworkCore;

namespace api_sistema_de_chamado.Repositories.Usuario
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExisteUsuarioOuEmailAsync(string nome, string email)
        {
            return !await _context.Usuario.AnyAsync(u => u.Nome == nome || u.Email == email);
        }

        public async Task AdicionarAsync(UsuariosModel usuario)
        {
            await _context.Usuario.AddAsync(usuario);
        }

        public async Task SalvarAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<UsuariosModel?> ObterPorEmailAsync(string email)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Email == email);
        }

    }
}
