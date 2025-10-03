using Domain.Models;

namespace Domain.Interfaces
{
    public interface IChamadoRepository
    {
        Task<Chamado> AdicionarAsync(Chamado chamado);
        Task<bool> SalvarAsync();
        Task<List<Chamado>> ObterPorUsuarioAsync(int usuarioId);
        Task AtualizarAsync(Chamado chamado);
        Task<Chamado?> ObterIdAsync(int id);
        Task<List<Chamado>> ObterTodosAsync();
    }
}