using Application.Dtos;

namespace Application.Interface
{
    public interface IChamado
    {
        Task<ChamadoLeituraDto>FazerChamadoAsync(FazerChamadoDto dto, int usuarioId);
        Task<List<ChamadoLeituraDto>> ListarChamadosPorUsuarioAsync(int usuarioId);
        Task<ChamadoLeituraDto> ResponderChamadoAsync(int id, ResponderChamadoDto dto);
        Task<List<ChamadoLeituraDto>> ListarTodosOsChamadosAsync();
    }
}
