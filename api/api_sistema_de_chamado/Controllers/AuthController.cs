using api_sistema_de_chamado.Dtos;
using api_sistema_de_chamado.Services.AuthService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api_sistema_de_chamado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthInterface _authService;
        public AuthController(IAuthInterface authService)
        {
            _authService = authService;
        }

        [HttpPost("register-user")]
        public async Task<ActionResult> RegisterUser([FromBody] UsuarioCriacaoDto dto)
        {
            var resultado = await _authService.RegistrarUsuario(dto);
            return Ok(resultado);
        }

        [HttpPost("register-admin")]
        public async Task<ActionResult> RegisterAdmin([FromBody] UsuarioCriacaoDto dto)
        {
            var resultado = await _authService.RegistrarAdmin(dto);
            return Ok(resultado);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UsuarioLoginDto usuarioLogin)
        {
            var resposta = await _authService.Login(usuarioLogin);
            return Ok(resposta);
        }

    }
}
