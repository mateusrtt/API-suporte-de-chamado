using Domain.Models;

namespace Domain.Interfaces
{
    public interface IAcessoLogRepository
    {
        Task<AcessoLog> AdicionarAsync(AcessoLog log);
        Task<bool> SalvarAsync();
    }
}