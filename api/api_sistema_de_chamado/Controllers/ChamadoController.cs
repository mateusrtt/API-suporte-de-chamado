using Application.Dtos;
using Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_sistema_de_chamado.Controllers
{
    [Route("suporte/v1/chamados")]
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
        public async Task<ActionResult<ChamadoLeituraDto>> FazerChamado([FromBody] FazerChamadoDto dto)
        {
            int usuarioId = int.Parse(User.FindFirst("Id")?.Value ?? "0");
            var resultado = await _chamadoService.FazerChamadoAsync(dto, usuarioId);
            HttpContext.Items["MensagemSucesso"] = "Chamado criado com sucesso!";
            return Ok(resultado);
        }

        [Authorize(Roles = "Usuario")]
        [HttpGet("meus")]
        public async Task<ActionResult<List<ChamadoLeituraDto>>> ListarMeusChamados()
        {
            int usuarioId = int.Parse(User.FindFirst("Id")?.Value ?? "0");
            var resultado = await _chamadoService.ListarChamadosPorUsuarioAsync(usuarioId);
            HttpContext.Items["MensagemSucesso"] = "Chamados carregados com sucesso!";
            return Ok(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ChamadoLeituraDto>> ResponderChamado(int id, [FromBody] ResponderChamadoDto dto)
        {
            var resultado = await _chamadoService.ResponderChamadoAsync(id, dto);
            HttpContext.Items["MensagemSucesso"] = "Chamado respondido com sucesso!";
            return Ok(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<ActionResult<List<ChamadoLeituraDto>>> ListarTodosChamados()
        {
            var resultado = await _chamadoService.ListarTodosOsChamadosAsync();
            HttpContext.Items["MensagemSucesso"] = "Todos os chamados carregados com sucesso!";
            return Ok(resultado);
        }
    }
}