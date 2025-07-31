using api_sistema_de_chamado.Data;
using api_sistema_de_chamado.Models;
using Microsoft.EntityFrameworkCore;

namespace api_sistema_de_chamado.Repositories.Chamado
{
    public class ChamadoRepository : IChamadoRepository
    {
        private readonly AppDbContext _context;

        public ChamadoRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<ChamadoModel> AdicionarAsync(ChamadoModel chamado)
        {
            _context.Chamado.Add(chamado);
            return Task.FromResult(chamado);
        }

        public async Task<List<ChamadoModel>> ObterPorUsuarioAsync(int usuarioId)
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
    }
}
