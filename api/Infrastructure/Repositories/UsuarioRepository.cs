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

        // Adiciona um usuário ao contexto
        public async Task Adicionar(Usuario usuario)
        {
            await _context.Usuario.AddAsync(usuario);
        }

        // Marca o usuário para atualização
        public Task Atualizar(Usuario usuario)
        {
            _context.Usuario.Update(usuario);
            return Task.CompletedTask;
        }

        // Remove o usuário do contexto
        public void Deletar(Usuario usuario)
        {
            _context.Usuario.Remove(usuario);
        }

        // Verifica se o e-mail já está cadastrado
        public async Task<bool> EmailJaCadastrado(string email)
        {
            return await _context.Usuario.AnyAsync(u => u.Email == email);
        }

        // Obtém usuário por e-mail
        public async Task<Usuario?> ObterPorEmail(string email)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Email == email);
        }

        // Obtém usuário por ID
        public async Task<Usuario?> ObterPorId(int id)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Id == id);
        }

        // Salva alterações no banco
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