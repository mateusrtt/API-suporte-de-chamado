using api_sistema_de_chamado.Models;

namespace api_sistema_de_chamado.Repositories.Usuario
{
    public interface IUsuarioRepository
    {
        Task<bool> ExisteUsuarioOuEmailAsync(string nome, string email);
        Task AdicionarAsync(UsuariosModel usuario);
        Task SalvarAsync();
    }
}
