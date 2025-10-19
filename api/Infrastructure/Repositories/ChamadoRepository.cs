using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Infrastructure.Exceptions;

namespace Infrastructure.Repositories
{
    public class ChamadoRepository : IChamadoRepository
    {
        private readonly AppDbContext _context;

        public ChamadoRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<Chamado> Adicionar(Chamado chamado)
        {
            _context.Chamado.Add(chamado);
            return Task.FromResult(chamado);
        }

        public Task Atualizar(Chamado chamado)
        {
            _context.Chamado.Update(chamado);
            return Task.CompletedTask;
        }

        public async Task<List<Chamado>> ObterPorUsuario(int usuarioId)
        {
            return await _context.Chamado
                .Include(c => c.Usuario)
                .Where(c => c.UsuarioId == usuarioId)
                .OrderByDescending(c => c.ChamadoDataCriacao)
                .ToListAsync();
        }

        public async Task<Chamado?> ObterId(int id)
        {
            return await _context.Chamado
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Chamado>> ObterTodos()
        {
            return await _context.Chamado
                .Include(c => c.Usuario)
                .OrderByDescending(c => c.ChamadoDataCriacao)
                .ToListAsync();
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