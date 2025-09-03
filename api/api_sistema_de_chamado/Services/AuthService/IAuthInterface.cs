using api_sistema_de_chamado.Dtos;
using api_sistema_de_chamado.Enums;
using api_sistema_de_chamado.Models;

namespace api_sistema_de_chamado.Services.AuthService
{
    public interface IAuthInterface
    {
        Task<UsuarioCriacaoDto> Registrar(UsuarioCriacaoDto usuarioRegistro, CargoEnum cargo);
        Task<String> Login(UsuarioLoginDto usuarioLogin);
    }
}