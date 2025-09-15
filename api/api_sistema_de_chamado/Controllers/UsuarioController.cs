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
        public async Task<ActionResult<UsuarioLeituraDto>> Atualizar(int id, [FromBody] UsuarioUpdateDto dto)
        {
            var usuarioLogado = (UsuarioLogAcessoDto)HttpContext.Items["UsuarioLogAcesso"]!;
            var resultado = await _usuarioService.Atualizar(id, dto, usuarioLogado.UsuarioId);
            HttpContext.Items["MensagemSucesso"] = "Dados atualizados com sucesso!";
            return Ok(resultado);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Usuario,Administrador")]
        public async Task<ActionResult<bool>> Deletar(int id)
        {
            var usuarioLogado = (UsuarioLogAcessoDto)HttpContext.Items["UsuarioLogAcesso"]!;
            bool sucesso = await _usuarioService.Deletar(id, usuarioLogado.UsuarioId);

            if (sucesso)
                HttpContext.Items["MensagemSucesso"] = "Conta deletada com sucesso!";

            return Ok(sucesso);
        }
    }
}