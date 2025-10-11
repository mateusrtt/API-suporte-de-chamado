using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Exceptions;

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
            try
            {
                var resultado = await _context.SaveChangesAsync();
                if (resultado <= 0)
                    throw new PersistenciaException("Nenhuma alteração foi salva no banco de dados.");
            }
            catch (DbUpdateException)
            {
                throw new BancoDeDadosException("Erro ao salvar alterações no banco.");
            }
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