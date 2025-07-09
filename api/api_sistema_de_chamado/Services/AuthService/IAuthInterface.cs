using api_sistema_de_chamado.Dtos;
using api_sistema_de_chamado.Models;

namespace api_sistema_de_chamado.Services.AuthService
{
    public interface IAuthInterface
    {
        Task<Response<UsuarioCriacaoDto>> RegistrarUsuario(UsuarioCriacaoDto usuarioRegistro);
        Task<Response<UsuarioCriacaoDto>> RegistrarAdmin(UsuarioCriacaoDto usuarioRegistro);
        Task<Response<String>> Login(UsuarioLoginDto usuarioLogin);
    }

}

