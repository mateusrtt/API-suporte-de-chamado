using api_sistema_de_chamado.Models;

namespace api_sistema_de_chamado.Repositories.AcessoLog
{
    public interface IAcessoLogRepository
    {
        Task<AcessoLogModel> AdicionarAsync(AcessoLogModel log);
        Task<bool> SalvarAsync();
    }
}
