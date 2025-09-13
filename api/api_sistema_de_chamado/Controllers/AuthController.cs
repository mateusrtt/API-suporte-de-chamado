using Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Domain.Enums;

namespace api_sistema_de_chamado.Controllers
{
    [Route("suporte/v1/usuarios")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;
        public AuthController(IAuth authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUser([FromBody] UsuarioCriacaoDto dto)
        {
            var resultado = await _authService.Registrar(dto, CargoEnum.Usuario);
            HttpContext.Items["MensagemSucesso"] = "Usuário cadastrado com sucesso!";
            return Ok(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("admins")]
        public async Task<ActionResult> RegisterAdmin([FromBody] UsuarioCriacaoDto dto)
        {
            var resultado = await _authService.Registrar(dto, CargoEnum.Administrador);
            HttpContext.Items["MensagemSucesso"] = "Administrador cadastrado com sucesso!";
            return Ok(resultado);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UsuarioLoginDto usuarioLogin)
        {
            var resultado = await _authService.Login(usuarioLogin);
            HttpContext.Items["MensagemSucesso"] = "Login realizado com sucesso!";
            return Ok(resultado);
        }
    }
}