using api_sistema_de_chamado.Dtos;
using api_sistema_de_chamado.Models;

namespace api_sistema_de_chamado.Services.ChamadoService
{
    public interface IChamadoInterface
    {
        Task<ChamadoLeituraDto>FazerChamadoAsync(FazerChamadoDto dto, int usuarioId);
        Task<List<ChamadoLeituraDto>> ListarChamadosPorUsuarioAsync(int usuarioId);
        Task<ChamadoLeituraDto> ResponderChamadoAsync(int id, ResponderChamadoDto dto);
        Task<List<ChamadoLeituraDto>> ListarTodosOsChamadosAsync();
    }
}
