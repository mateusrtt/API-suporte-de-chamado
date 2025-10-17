using Application.Dtos;
using Domain.Enums;

namespace Application.Interface
{
    public interface IUsuario
    {
        Task<UsuarioLeituraDto> Registrar(UsuarioCriacaoDto usuarioRegistro, CargoEnum cargo = CargoEnum.Usuario);
        Task<UsuarioLeituraDto> Atualizar(int id, UsuarioUpdateDto dto, int usuarioLogadoId);
        Task<bool>Deletar(int id, int usuarioId);
    }
}