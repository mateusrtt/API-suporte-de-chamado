using api_sistema_de_chamado.Dtos;
using api_sistema_de_chamado.Enum;
using api_sistema_de_chamado.Models;
using api_sistema_de_chamado.Repositories.Chamado;
using api_sistema_de_chamado.Services.OpenAiService;

namespace api_sistema_de_chamado.Services.ChamadoService
{
    public class ChamadoService : IChamadoInterface
    {
        private readonly IChamadoRepository _chamadoRepository;
        private readonly IOpenAiInterface _openAiService;

        public ChamadoService(IChamadoRepository chamadoRepository, IOpenAiInterface openAiService)
        {
            _chamadoRepository = chamadoRepository;
            _openAiService = openAiService;
        }
        public async Task<ChamadoLeituraDto> FazerChamadoAsync(FazerChamadoDto dto, int usuarioId)
        {
            if (string.IsNullOrWhiteSpace(dto.Descricao))
                throw new ApplicationException("A descrição do chamado é obrigatória.");

            int prioridadeNumero = await _openAiService.ObterPrioridadePorDescricaoAsync(dto.Descricao);

            if (!System.Enum.IsDefined(typeof(PrioridadeEnum), prioridadeNumero))
                throw new ApplicationException("Prioridade inválida retornada pela IA.");

            ChamadoModel chamado = new ChamadoModel(
                 dto.Descricao,
                 usuarioId,
                 (PrioridadeEnum)prioridadeNumero
            );

            await _chamadoRepository.AdicionarAsync(chamado);
            await _chamadoRepository.SalvarAsync();

            return new ChamadoLeituraDto
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

        public async Task<List<ChamadoLeituraDto>> ListarChamadosPorUsuarioAsync(int usuarioId)
        {
            var chamados = await _chamadoRepository.ObterPorUsuarioAsync(usuarioId);

            if (chamados == null || !chamados.Any())
                throw new ApplicationException("Nenhum chamado encontrado para este usuário.");

            return chamados.Select(c => new ChamadoLeituraDto
            {
                Id = c.Id,
                Descricao = c.Descricao,
                Resposta = c.Resposta,
                Status = c.Status,
                ChamadoDataCriacao = c.ChamadoDataCriacao,
                UsuarioId = c.UsuarioId,
                Prioridade = c.Prioridade
            }).ToList();
        }

        public async Task<ChamadoLeituraDto> ResponderChamadoAsync(int id, ResponderChamadoDto dto)
        {
            var chamado = await _chamadoRepository.ObterIdAsync(id);

            if (chamado == null)
                throw new ApplicationException("Chamado não encontrado.");

            if (chamado.Status == StatusChamadoEnum.Respondido)
                throw new ApplicationException("Este chamado já foi respondido e não pode ser respondido novamente.");

            chamado.Responder(dto.Resposta);

            await _chamadoRepository.AtualizarAsync(chamado);
            await _chamadoRepository.SalvarAsync();

            return new ChamadoLeituraDto
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

        public async Task<List<ChamadoLeituraDto>> ListarTodosOsChamadosAsync()
        {
            var chamados = await _chamadoRepository.ObterTodosAsync();

            if (chamados == null || !chamados.Any())
                throw new ApplicationException("Nenhum chamado encontrado.");

            return chamados.Select(c => new ChamadoLeituraDto
            {
                Id = c.Id,
                Descricao = c.Descricao,
                Resposta = c.Resposta,
                Status = c.Status,
                ChamadoDataCriacao = c.ChamadoDataCriacao,
                UsuarioId = c.UsuarioId,
                Prioridade = c.Prioridade
            }).ToList();
        }
    }
}