using Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Domain.Enums;
using Api.Attributes;

namespace Api.Controllers
{
    [Route("suporte/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;
        public AuthController(IAuth authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [MensagemSucessoAtributo("Usuário cadastrado com sucesso!")]
        public async Task<ActionResult> RegisterUser([FromBody] UsuarioCriacaoDto dto)
        {
            var resultado = await _authService.Registrar(dto, CargoEnum.Usuario, registrarLog: true);
            return Ok(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("admins")]
        [MensagemSucessoAtributo("Administrador cadastrado com sucesso!")]
        public async Task<ActionResult> RegisterAdmin([FromBody] UsuarioCriacaoDto dto)
        {
            var resultado = await _authService.Registrar(dto, CargoEnum.Administrador, registrarLog: false);
            return Ok(resultado);
        }

        [HttpPost("login")]
        [MensagemSucessoAtributo("Login realizado com sucesso!")]
        public async Task<ActionResult> Login(UsuarioLoginDto usuarioLogin)
        {
            var resultado = await _authService.Login(usuarioLogin);
            return Ok(resultado);
        }
    }
}