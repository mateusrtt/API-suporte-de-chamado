using Application.Dtos;
using Application.Interface;
using Application.Utils;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services
{
    // Serviços responsável dos chamados
    public class ChamadoService : IChamado
    {
        private readonly IChamadoRepository _chamadoRepository;
        private readonly IOpenAi _openAiService;

        public ChamadoService(IChamadoRepository chamadoRepository, IOpenAi openAiService)
        {
            _chamadoRepository = chamadoRepository;
            _openAiService = openAiService;
        }

        /// <summary>
        /// Cria um novo chamado, consulta a IA para definir a prioridade com base na descrição
        /// e salva o chamado no repositório
        /// </summary>
        /// <param name="dto">Dados enviados pelo usuário contendo a descrição do chamado.</param>
        /// <param name="usuarioId">ID do usuário que abriu o chamado.</param>
        /// <returns>DTO com os dados do chamado criado.</returns>
        /// <exception cref="ServicoIndisponivelException">Lançado quando não há conexão com a internet.</exception>
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

        /// <summary>
        /// Lista todos os chamados pertencentes a um usuário específico
        /// </summary>
        /// <param name="usuarioId">ID do usuário.</param>
        /// <returns>Lista de chamados do usuário.</returns>
        /// <exception cref="NaoEncontradoException">Lançado quando o usuário não possui chamados.</exception>
        public async Task<List<ChamadoLeituraDto>> ListarChamadosPorUsuario(int usuarioId)
        {
            var chamados = await _chamadoRepository.ObterPorUsuario(usuarioId);

            if (chamados == null || !chamados.Any())
                throw new NaoEncontradoException("Nenhum chamado encontrado para este usuário.");

            return chamados.Select(MapParaDto).ToList();
        }

        /// <summary>
        /// Registra uma resposta para um chamado existente e atualiza seu status
        /// </summary>
        /// <param name="id">ID do chamado.</param>
        /// <param name="dto">DTO contendo a resposta ao chamado.</param>
        /// <returns>DTO atualizado do chamado.</returns>
        /// <exception cref="NaoEncontradoException">Lançado quando o chamado não existe.</exception>
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

        /// <summary>
        /// Lista todos os chamados cadastrados no sistema.
        /// </summary>
        /// <returns>Lista de todos os chamados.</returns>
        /// <exception cref="NaoEncontradoException">Lançado quando não há chamados cadastrados.</exception>
        public async Task<List<ChamadoLeituraDto>> ListarTodosOsChamados()
        {
            var chamados = await _chamadoRepository.ObterTodos();

            if (chamados == null || !chamados.Any())
                throw new NaoEncontradoException("Nenhum chamado encontrado.");

            return chamados.Select(MapParaDto).ToList();
        }

        // Converte um objeto de domínio Chamado em DTO para leitura
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