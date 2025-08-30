using api_sistema_de_chamado.Data;
using api_sistema_de_chamado.Models;

namespace api_sistema_de_chamado.Repositories.AcessoLog
{
    public class AcessoLogRepository : IAcessoLogRepository
    {
        private readonly AppDbContext _context;

        public AcessoLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<AcessoLogModel> AdicionarAsync(AcessoLogModel log)
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