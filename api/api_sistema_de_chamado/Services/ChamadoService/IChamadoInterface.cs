using api_sistema_de_chamado.Dtos;
using api_sistema_de_chamado.Models;

namespace api_sistema_de_chamado.Services.ChamadoService
{
    public interface IChamadoInterface
    {
        Task<Response<ChamadoLeituraDto>> FazerChamadoAsync(FazerChamadoDto dto, int usuarioId);
        Task<Response<List<ChamadoLeituraDto>>> ListarChamadosPorUsuarioAsync(int usuarioId);
    }
}
