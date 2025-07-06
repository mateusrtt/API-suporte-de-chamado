using api_sistema_de_chamado.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api_sistema_de_chamado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult> Register(UsuarioCriacaoDto usuarioRegister) 
        {
            return Ok();

        }

    }
}
