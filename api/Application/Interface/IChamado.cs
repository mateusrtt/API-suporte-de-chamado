using Application.Dtos;

namespace Application.Interface
{
    public interface IChamado
    {
        Task<ChamadoLeituraDto>FazerChamado(FazerChamadoDto dto, int usuarioId);
        Task<List<ChamadoLeituraDto>> ListarChamadosPorUsuario(int usuarioId);
        Task<ChamadoLeituraDto> ResponderChamado(int id, ResponderChamadoDto dto);
        Task<List<ChamadoLeituraDto>> ListarTodosOsChamados();
    }
}