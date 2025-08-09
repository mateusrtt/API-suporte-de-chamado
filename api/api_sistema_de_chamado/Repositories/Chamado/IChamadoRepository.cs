using api_sistema_de_chamado.Models;

namespace api_sistema_de_chamado.Repositories.Chamado
{
    public interface IChamadoRepository
    {
        Task<ChamadoModel> AdicionarAsync(ChamadoModel chamado);
        Task<bool> SalvarAsync();
        Task<List<ChamadoModel>> ObterPorUsuarioAsync(int usuarioId);
        Task AtualizarAsync(ChamadoModel chamado);
        Task<ChamadoModel?> ObterIdAsync(int id);
        Task<List<ChamadoModel>> ObterTodosAsync();
    }
}
