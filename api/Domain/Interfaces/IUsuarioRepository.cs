using Domain.Models;

namespace Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<bool> ExisteUsuarioOuEmailAsync(string nome, string email);
        Task AdicionarAsync(Usuario usuario);
        Task SalvarAsync();
        Task<Usuario?> ObterPorEmailAsync(string email);
    }
}