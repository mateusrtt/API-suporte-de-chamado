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
        public async Task<Response<ChamadoLeituraDto>> FazerChamadoAsync(FazerChamadoDto dto, int usuarioId)
        {
            Response<ChamadoLeituraDto> respostaServico = new Response<ChamadoLeituraDto>();

            try
            {
                int prioridadeNumero = await _openAiService.ObterPrioridadePorDescricaoAsync(dto.Descricao);

                ChamadoModel chamado = new ChamadoModel(
                     dto.Descricao,
                     usuarioId,
                     (PrioridadeEnum)prioridadeNumero
                );

                await _chamadoRepository.AdicionarAsync(chamado);
                await _chamadoRepository.SalvarAsync();

                // Monta o DTO para retorno
                var chamadoDto = new ChamadoLeituraDto
                {
                    Id = chamado.Id,
                    Descricao = chamado.Descricao,
                    Resposta = chamado.Resposta,
                    Status = chamado.Status,
                    ChamadoDataCriacao = chamado.ChamadoDataCriacao,
                    UsuarioId = chamado.UsuarioId,
                    Prioridade = chamado.Prioridade
                };

                respostaServico.Mensagem = "Chamado criado com sucesso!";
                respostaServico.Status = true;
                respostaServico.Dados = chamadoDto;  
            }
            catch (System.Exception ex)
            {
                var innerMsg = ex.InnerException?.Message ?? "";
                respostaServico.Status = false;
                respostaServico.Mensagem = $"Erro ao criar chamado: {ex.Message} | Detalhes: {innerMsg}";
                respostaServico.Dados = null;
            }
            return respostaServico;
        }

        public async Task<Response<List<ChamadoLeituraDto>>> ListarChamadosPorUsuarioAsync(int usuarioId)
        {
            Response<List<ChamadoLeituraDto>> respostaServico = new Response<List<ChamadoLeituraDto>>();

            try
            {
                var chamados = await _chamadoRepository.ObterPorUsuarioAsync(usuarioId);

                var listaDto = chamados.Select(c => new ChamadoLeituraDto
                {
                    Id = c.Id,
                    Descricao = c.Descricao,
                    Resposta = c.Resposta,
                    Status = c.Status,
                    ChamadoDataCriacao = c.ChamadoDataCriacao,
                    UsuarioId = c.UsuarioId,
                    Prioridade = c.Prioridade
                }).ToList();

                respostaServico.Dados = listaDto;
                respostaServico.Status = true;
            }
            catch (System.Exception ex)
            {
                respostaServico.Status = false;
                respostaServico.Mensagem = $"Erro ao listar chamados: {ex.Message}";
                respostaServico.Dados = null;
            }
            return respostaServico;
        }

        public async Task<Response<ChamadoLeituraDto>> ResponderChamadoAsync(int id, ResponderChamadoDto dto)
        {
            var respostaServico = new Response<ChamadoLeituraDto>();

            try
            {
                var chamado = await _chamadoRepository.ObterIdAsync(id);

                if (chamado == null)
                {
                    respostaServico.Status = false;
                    respostaServico.Mensagem = "Chamado não encontrado.";
                    return respostaServico;
                }

                if (chamado.Status == StatusChamadoEnum.Respondido)
                {
                    respostaServico.Status = false;
                    respostaServico.Mensagem = "Este chamado já foi respondido e não pode ser respondido novamente";
                    return respostaServico;
                }

                // Define a resposta e força o status como Respondido
                chamado.Responder(dto.Resposta);

                await _chamadoRepository.AtualizarAsync(chamado);
                await _chamadoRepository.SalvarAsync();

                var chamadoDto = new ChamadoLeituraDto
                {
                    Id = chamado.Id,
                    Descricao = chamado.Descricao,
                    Resposta = chamado.Resposta,
                    Status = chamado.Status,
                    ChamadoDataCriacao = chamado.ChamadoDataCriacao,
                    UsuarioId = chamado.UsuarioId,
                    Prioridade = chamado.Prioridade
                };

                respostaServico.Status = true;
                respostaServico.Mensagem = "Chamado respondido com sucesso!";
                respostaServico.Dados = chamadoDto;
            }
            catch (Exception ex)
            {
                respostaServico.Status = false;
                respostaServico.Mensagem = $"Erro ao responder chamado: {ex.Message}";
            }
            return respostaServico;
        }

        public async Task<Response<List<ChamadoLeituraDto>>> ListarTodosOsChamadosAsync()
        {
            var respostaServico = new Response<List<ChamadoLeituraDto>>();

            try
            {
                var chamados = await _chamadoRepository.ObterTodosAsync();

                var listaDto = chamados.Select(c => new ChamadoLeituraDto
                {
                    Id = c.Id,
                    Descricao = c.Descricao,
                    Resposta = c.Resposta,
                    Status = c.Status,
                    ChamadoDataCriacao = c.ChamadoDataCriacao,
                    UsuarioId = c.UsuarioId,
                    Prioridade = c.Prioridade
                }).ToList();

                respostaServico.Dados = listaDto;
                respostaServico.Status = true;
            }
            catch (Exception ex)
            {
                respostaServico.Status = false;
                respostaServico.Mensagem = $"Erro ao listar todos os chamados: {ex.Message}";
                respostaServico.Dados = null;
            }
            return respostaServico;
        }
    }
}
