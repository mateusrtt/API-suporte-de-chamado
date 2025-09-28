using Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
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

        [HttpPost("login")]
        [MensagemSucessoAtributo("Login realizado com sucesso!")]
        public async Task<ActionResult> Login(UsuarioLoginDto usuarioLogin)
        {
            var resultado = await _authService.Login(usuarioLogin);
            return Ok(resultado);
        }
    }
}