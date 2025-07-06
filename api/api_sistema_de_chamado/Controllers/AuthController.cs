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
        private readonly IAuthInterface _authInterface;
        public AuthController(IAuthInterface authInterface)
        {
            _authInterface = authInterface;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UsuarioCriacaoDto usuarioRegister) 
        {

            var resposta = await _authInterface.Registrar(usuarioRegister);
            return Ok(resposta);
        }

    }
}
