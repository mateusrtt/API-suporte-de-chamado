using Api.Attributes;
using Application.Dtos;
using Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("suporte/v1/usuario")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuario _usuarioService;

        public UsuarioController(IUsuario usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Usuario,Administrador")]
        [MensagemSucessoAtributo("Dados atualizados com sucesso!")]
        public async Task<ActionResult> Atualizar(int id, [FromBody] UsuarioUpdateDto dto)
        {
            var usuarioIdString = HttpContext.User.FindFirst("Id")?.Value;
            if (!int.TryParse(usuarioIdString, out var usuarioLogadoId))
                return Unauthorized();

            await _usuarioService.Atualizar(id, dto, usuarioLogadoId);

            return Ok(true);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Usuario,Administrador")]
        [MensagemSucessoAtributo("Conta deletada com sucesso!")]
        public async Task<ActionResult<bool>> Deletar(int id)
        {
            var usuarioIdString = HttpContext.User.FindFirst("Id")?.Value;
            if (!int.TryParse(usuarioIdString, out var usuarioLogadoId))
                return Unauthorized();

            await _usuarioService.Deletar(id, usuarioLogadoId);

            return Ok(true);
        }
    }
}