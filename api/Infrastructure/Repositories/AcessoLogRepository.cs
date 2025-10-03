using Domain.Interfaces;
using Infrastructure.Data;
using Domain.Models;

namespace Infrastructure.Repositories
{
    public class AcessoLogRepository : IAcessoLogRepository
    {
        private readonly AppDbContext _context;

        public AcessoLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<AcessoLog> AdicionarAsync(AcessoLog log)
        {
            _context.AcessoLogs.Add(log);
            return Task.FromResult(log);
        }

        public async Task<bool> SalvarAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}