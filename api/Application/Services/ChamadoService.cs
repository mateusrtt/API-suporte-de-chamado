using Application.Dtos;
using Application.Interface;
using Application.Utils;
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
        public async Task<ChamadoLeituraDto> FazerChamado(FazerChamadoDto dto, int usuarioId)
        {
            if (!await VerificarConexao.TemInternet())
                throw new ServicoIndisponivelException("Sem conexão com a internet. Verifique e tente novamente.");

            var chamado = new Chamado(dto.Descricao, usuarioId);

            int prioridadeNumero = await _openAiService.ObterPrioridadePorDescricao(dto.Descricao);

            chamado.AtualizarPrioridade((PrioridadeEnum)prioridadeNumero);

            await _chamadoRepository.Adicionar(chamado);
            await _chamadoRepository.Salvar();

            return MapParaDto(chamado);
        }

        public async Task<List<ChamadoLeituraDto>> ListarChamadosPorUsuario(int usuarioId)
        {
            var chamados = await _chamadoRepository.ObterPorUsuario(usuarioId);

            if (chamados == null || !chamados.Any())
                throw new NaoEncontradoException("Nenhum chamado encontrado para este usuário.");

            return chamados.Select(MapParaDto).ToList();
        }

        public async Task<ChamadoLeituraDto> ResponderChamado(int id, ResponderChamadoDto dto)
        {
            var chamado = await _chamadoRepository.ObterId(id);

            if (chamado == null)
                throw new NaoEncontradoException("Chamado não encontrado.");

            chamado.Responder(dto.Resposta);

            await _chamadoRepository.Atualizar(chamado);
            await _chamadoRepository.Salvar();

            return MapParaDto(chamado);
        }

        public async Task<List<ChamadoLeituraDto>> ListarTodosOsChamados()
        {
            var chamados = await _chamadoRepository.ObterTodos();

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
                Prioridade = chamado.Prioridade,
                Nome = chamado.Usuario?.Nome ?? string.Empty
            };
    }
}