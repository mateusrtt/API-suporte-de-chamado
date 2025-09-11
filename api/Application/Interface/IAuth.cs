using Application.Dtos;
using Domain.Enums;

namespace Application.Interface
{
    public interface IAuth
    {
        Task<UsuarioCriacaoDto> Registrar(UsuarioCriacaoDto usuarioRegistro, CargoEnum cargo);
        Task<string> Login(UsuarioLoginDto usuarioLogin);
    }
}