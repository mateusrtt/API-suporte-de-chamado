using Application.Dtos;

namespace Application.Interface
{
    public interface IAuth
    {
        Task<UsuarioLoginRespostaDto> Login(UsuarioLoginDto usuarioLogin);
    }
}