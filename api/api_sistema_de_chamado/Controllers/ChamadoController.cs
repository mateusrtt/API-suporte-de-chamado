using api_sistema_de_chamado.Dtos;
using api_sistema_de_chamado.Models;
using api_sistema_de_chamado.Services.ChamadoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace api_sistema_de_chamado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChamadoController : ControllerBase
    {
        private readonly IChamadoInterface _chamadoService;

        public ChamadoController(IChamadoInterface chamadoService)
        {
            _chamadoService = chamadoService;
        }

        [Authorize(Roles = "Usuario")]
        [HttpPost("fazer")]
        public async Task<ActionResult<Response<ChamadoLeituraDto>>> FazerChamado([FromBody] FazerChamadoDto dto)
        {
            int usuarioId = int.Parse(User.FindFirst("Id")?.Value ?? "0");
            var resultado = await _chamadoService.FazerChamadoAsync(dto, usuarioId);
            return Ok(resultado);
        }

        [Authorize(Roles = "Usuario")]
        [HttpGet("meus")]
        public async Task<ActionResult<Response<List<ChamadoLeituraDto>>>> ListarMeusChamados()
        {
            int usuarioId = int.Parse(User.FindFirst("Id")?.Value ?? "0");
            var resultado = await _chamadoService.ListarChamadosPorUsuarioAsync(usuarioId);
            return Ok(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("responder")]
        public async Task<ActionResult<Response<ChamadoLeituraDto>>> ResponderChamado([FromBody] ResponderChamadoDto dto)
        {
            var resultado = await _chamadoService.ResponderChamadoAsync(dto);
            return Ok(resultado);
        }
    }
}
