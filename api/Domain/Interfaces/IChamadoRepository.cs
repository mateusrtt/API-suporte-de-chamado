using Domain.Models;

namespace Domain.Interfaces
{
    public interface IChamadoRepository
    {
        Task<Chamado> Adicionar(Chamado chamado);
        Task Atualizar(Chamado chamado);
        Task<List<Chamado>> ObterPorUsuario(int usuarioId);
        Task<Chamado?> ObterId(int id);
        Task<List<Chamado>> ObterTodos();
        Task Salvar();
    }
}