using Application.Dtos; 

namespace Application.Interface
{
    public interface IUsuario
    {
        Task<UsuarioLeituraDto> Atualizar(int id, UsuarioUpdateDto dto, int usuarioLogadoId);
        Task<bool>Deletar(int id, int usuarioId);
    }
}