using Application.Dtos;
using Domain.Enums;

namespace Application.Interface
{
    public interface IAuth
    {
        Task<UsuarioLeituraDto> Registrar(UsuarioCriacaoDto usuarioRegistro, CargoEnum cargo, bool registrarLog = true);
        Task<UsuarioLoginRespostaDto> Login(UsuarioLoginDto usuarioLogin);
    }
}