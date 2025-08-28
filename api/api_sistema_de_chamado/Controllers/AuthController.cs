using api_sistema_de_chamado.Dtos;
using api_sistema_de_chamado.Models;
using api_sistema_de_chamado.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api_sistema_de_chamado.Controllers
{
    [Route("suporte/v1/usuarios")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthInterface _authService;
        public AuthController(IAuthInterface authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUser([FromBody] UsuarioCriacaoDto dto)
        {
            var resultado = await _authService.RegistrarUsuario(dto);
            HttpContext.Items["MensagemSucesso"] = "Usuário cadastrado com sucesso!";
            return Ok(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("admins")]
        public async Task<ActionResult> RegisterAdmin([FromBody] UsuarioCriacaoDto dto)
        {
            var resultado = await _authService.RegistrarAdmin(dto);
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
