using Domain.Models;

namespace Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task Adicionar(Usuario usuario);
        Task Atualizar(Usuario usuario);
        void Deletar(Usuario usuario);
        Task<bool> EmailJaCadastrado(string email);
        Task<Usuario?> ObterPorEmail(string email);
        Task<Usuario?> ObterPorId(int id);
        Task Salvar();
    }
}