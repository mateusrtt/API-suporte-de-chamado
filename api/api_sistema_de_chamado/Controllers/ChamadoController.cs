using Api.Attributes;
using Application.Dtos;
using Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Controller responsável pelos endpoints de chamados.
    /// Permite criar, listar e responder chamados, controlando acesso por perfil de usuário.
    /// </summary>
    [Route("suporte/v1/chamado")]
    [ApiController]
    public class ChamadoController : ControllerBase
    {
        private readonly IChamado _chamadoService;

        /// <summary>
        /// Construtor que injeta o serviço de chamados.
        /// </summary>
        /// <param name="chamadoService">Serviço de chamados (IChamado) usado para processar as operações.</param>
        public ChamadoController(IChamado chamadoService)
        {
            _chamadoService = chamadoService;
        }

        /// <summary>
        /// Cria um novo chamado para o usuário logado.
        /// </summary>
        /// <param name="dto">DTO contendo informações do chamado.</param>
        /// <returns>ActionResult com o chamado criado, encapsulado pelo filtro de sucesso.</returns>
        [Authorize(Roles = "Usuario")]
        [HttpPost]
        [MensagemSucessoAtributo("Chamado criado com sucesso!")]
        public async Task<ActionResult<ChamadoLeituraDto>> FazerChamado([FromBody] FazerChamadoDto dto)
        {
            int usuarioId = int.Parse(User.FindFirst("Id")?.Value ?? "0");
            var resultado = await _chamadoService.FazerChamado(dto, usuarioId);
            return Ok(resultado);
        }

        /// <summary>
        /// Lista os chamados do usuário logado.
        /// </summary>
        /// <returns>ActionResult com a lista de chamados do usuário.</returns>
        [Authorize(Roles = "Usuario")]
        [HttpGet("meus")]
        [MensagemSucessoAtributo("Chamados carregados com sucesso!")]
        public async Task<ActionResult<List<ChamadoLeituraDto>>> ListarMeusChamados()
        {
            int usuarioId = int.Parse(User.FindFirst("Id")?.Value ?? "0");
            var resultado = await _chamadoService.ListarChamadosPorUsuario(usuarioId);
            return Ok(resultado);
        }

        /// <summary>
        /// Responde um chamado existente (apenas para administradores).
        /// </summary>
        /// <param name="id">ID do chamado a ser respondido.</param>
        /// <param name="dto">DTO contendo a resposta do chamado.</param>
        /// <returns>ActionResult com o chamado atualizado.</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        [MensagemSucessoAtributo("Chamado respondido com sucesso!")]
        public async Task<ActionResult<ChamadoLeituraDto>> ResponderChamado(int id, [FromBody] ResponderChamadoDto dto)
        {
            var resultado = await _chamadoService.ResponderChamado(id, dto);
            return Ok(resultado);
        }

        /// <summary>
        /// Lista todos os chamados do sistema (apenas para administradores).
        /// </summary>
        /// <returns>ActionResult com a lista de todos os chamados.</returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        [MensagemSucessoAtributo("Todos os chamados carregados com sucesso!")]
        public async Task<ActionResult<List<ChamadoLeituraDto>>> ListarTodosChamados()
        {
            var resultado = await _chamadoService.ListarTodosOsChamados();
            return Ok(resultado);
        }
    }
}