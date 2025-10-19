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

        public async Task Adicionar(Usuario usuario)
        {
            await _context.Usuario.AddAsync(usuario);
        }

        public Task Atualizar(Usuario usuario)
        {
            _context.Usuario.Update(usuario);
            return Task.CompletedTask;
        }

        public void Deletar(Usuario usuario)
        {
            _context.Usuario.Remove(usuario);
        }

        public async Task<bool> EmailJaCadastrado(string email)
        {
            return await _context.Usuario.AnyAsync(u => u.Email == email);
        }

        public async Task<Usuario?> ObterPorEmail(string email)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario?> ObterPorId(int id)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task Salvar()
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
    }
}