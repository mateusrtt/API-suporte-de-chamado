using api_sistema_de_chamado.Dtos;
using api_sistema_de_chamado.Models;

namespace api_sistema_de_chamado.Services.AuthService
{
    public interface IAuthInterface
    {
        Task<UsuarioCriacaoDto> RegistrarUsuario(UsuarioCriacaoDto usuarioRegistro);
        Task<UsuarioCriacaoDto> RegistrarAdmin(UsuarioCriacaoDto usuarioRegistro);
        Task<String> Login(UsuarioLoginDto usuarioLogin);
    }
}