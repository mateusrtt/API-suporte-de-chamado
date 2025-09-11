using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Infrastructure.Repositories
{
    public class ChamadoRepository : IChamadoRepository
    {
        private readonly AppDbContext _context;

        public ChamadoRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<Chamado> AdicionarAsync(Chamado chamado)
        {
            _context.Chamado.Add(chamado);
            return Task.FromResult(chamado);
        }

        public async Task<List<Chamado>> ObterPorUsuarioAsync(int usuarioId)
        {
            return await _context.Chamado
                .Include(c => c.Usuario)
                .Where(c => c.UsuarioId == usuarioId)
                .OrderByDescending(c => c.ChamadoDataCriacao)
                .ToListAsync();
        }

        public async Task<bool> SalvarAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Chamado?> ObterIdAsync(int id)
        {
            return await _context.Chamado
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task AtualizarAsync(Chamado chamado)
        {
            _context.Chamado.Update(chamado);
            return Task.CompletedTask;
        }

        public async Task<List<Chamado>> ObterTodosAsync()
        {
            return await _context.Chamado
                .Include(c => c.Usuario)
                .OrderByDescending(c => c.ChamadoDataCriacao)
                .ToListAsync();
        }
    }
}
