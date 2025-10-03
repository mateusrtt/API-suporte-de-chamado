using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories
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

        public async Task AdicionarAsync(Usuario usuario)
        {
            await _context.Usuario.AddAsync(usuario);
        }

        public async Task SalvarAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario?> ObterPorIdAsync(int id)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Id == id);
        }

        public void Atualizar(Usuario usuario)
        {
            _context.Usuario.Update(usuario);
        }

        public void Deletar(Usuario usuario)
        {
            _context.Usuario.Remove(usuario);
        }
    }
}