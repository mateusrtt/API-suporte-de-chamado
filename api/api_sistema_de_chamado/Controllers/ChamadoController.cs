using Api.Attributes;
using Application.Dtos;
using Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("suporte/v1/chamado")]
    [ApiController]
    public class ChamadoController : ControllerBase
    {
        private readonly IChamado _chamadoService;

        public ChamadoController(IChamado chamadoService)
        {
            _chamadoService = chamadoService;
        }

        [Authorize(Roles = "Usuario")]
        [HttpPost]
        [MensagemSucessoAtributo("Chamado criado com sucesso!")]
        public async Task<ActionResult<ChamadoLeituraDto>> FazerChamado([FromBody] FazerChamadoDto dto)
        {
            int usuarioId = int.Parse(User.FindFirst("Id")?.Value ?? "0");
            var resultado = await _chamadoService.FazerChamadoAsync(dto, usuarioId);
            return Ok(resultado);
        }

        [Authorize(Roles = "Usuario")]
        [HttpGet("meus")]
        [MensagemSucessoAtributo("Chamados carregados com sucesso!")]
        public async Task<ActionResult<List<ChamadoLeituraDto>>> ListarMeusChamados()
        {
            int usuarioId = int.Parse(User.FindFirst("Id")?.Value ?? "0");
            var resultado = await _chamadoService.ListarChamadosPorUsuarioAsync(usuarioId);
            return Ok(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        [MensagemSucessoAtributo("Chamado respondido com sucesso!")]
        public async Task<ActionResult<ChamadoLeituraDto>> ResponderChamado(int id, [FromBody] ResponderChamadoDto dto)
        {
            var resultado = await _chamadoService.ResponderChamadoAsync(id, dto);
            return Ok(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        [MensagemSucessoAtributo("Todos os chamados carregados com sucesso!")]
        public async Task<ActionResult<List<ChamadoLeituraDto>>> ListarTodosChamados()
        {
            var resultado = await _chamadoService.ListarTodosOsChamadosAsync();
            return Ok(resultado);
        }
    }
}