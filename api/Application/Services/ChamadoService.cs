using Application.Dtos;
using Application.Interface;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services
{
    public class ChamadoService : IChamado
    {
        private readonly IChamadoRepository _chamadoRepository;
        private readonly IOpenAi _openAiService;

        public ChamadoService(IChamadoRepository chamadoRepository, IOpenAi openAiService)
        {
            _chamadoRepository = chamadoRepository;
            _openAiService = openAiService;
        }
        public async Task<ChamadoLeituraDto> FazerChamadoAsync(FazerChamadoDto dto, int usuarioId)
        {
            int prioridadeNumero = await _openAiService.ObterPrioridadePorDescricaoAsync(dto.Descricao);
            if (!Enum.IsDefined(typeof(PrioridadeEnum), prioridadeNumero))
                prioridadeNumero = (int)PrioridadeEnum.Analisando;

            var chamado = new Chamado(dto.Descricao, usuarioId, (PrioridadeEnum)prioridadeNumero);

            await _chamadoRepository.AdicionarAsync(chamado);
            await _chamadoRepository.SalvarAsync();

            return MapParaDto(chamado);
        }

        public async Task<List<ChamadoLeituraDto>> ListarChamadosPorUsuarioAsync(int usuarioId)
        {
            var chamados = await _chamadoRepository.ObterPorUsuarioAsync(usuarioId);

            if (chamados == null || !chamados.Any())
                throw new NaoEncontradoException("Nenhum chamado encontrado para este usuário.");

            return chamados.Select(MapParaDto).ToList();
        }

        public async Task<ChamadoLeituraDto> ResponderChamadoAsync(int id, ResponderChamadoDto dto)
        {
            var chamado = await _chamadoRepository.ObterIdAsync(id);

            if (chamado == null)
                throw new NaoEncontradoException("Chamado não encontrado.");

            chamado.Responder(dto.Resposta);

            await _chamadoRepository.AtualizarAsync(chamado);
            await _chamadoRepository.SalvarAsync();

            return MapParaDto(chamado);
        }

        public async Task<List<ChamadoLeituraDto>> ListarTodosOsChamadosAsync()
        {
            var chamados = await _chamadoRepository.ObterTodosAsync();

            if (chamados == null || !chamados.Any())
                throw new NaoEncontradoException("Nenhum chamado encontrado.");

            return chamados.Select(MapParaDto).ToList();
        }

        private ChamadoLeituraDto MapParaDto(Chamado chamado) =>
            new ChamadoLeituraDto
            {
                Id = chamado.Id,
                Descricao = chamado.Descricao,
                Resposta = chamado.Resposta,
                Status = chamado.Status,
                ChamadoDataCriacao = chamado.ChamadoDataCriacao,
                UsuarioId = chamado.UsuarioId,
                Prioridade = chamado.Prioridade
            };
    }
}