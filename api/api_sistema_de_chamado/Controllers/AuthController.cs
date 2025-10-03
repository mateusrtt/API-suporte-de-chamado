using Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Api.Attributes;

namespace Api.Controllers
{
    /// <summary>
    /// Controller responsável pelos endpoints de autenticação da aplicação.
    /// Permite realizar login de usuários.
    /// </summary>
    [Route("suporte/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Construtor que injeta o serviço de autenticação.
        /// </summary>
        /// <param name="authService">Serviço de autenticação (IAuth) usado para processar login.</param>
        private readonly IAuth _authService;
        public AuthController(IAuth authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Realiza login do usuário.
        /// </summary>
        /// <param name="usuarioLogin">DTO contendo email/username e senha do usuário.</param>
        /// <returns>Retorna um ActionResult com os dados do login, encapsulados pelo filtro de sucesso.</returns>
        [HttpPost("login")]
        [MensagemSucessoAtributo("Login realizado com sucesso!")]
        public async Task<ActionResult> Login(UsuarioLoginDto usuarioLogin)
        {
            var resultado = await _authService.Login(usuarioLogin);
            return Ok(resultado);
        }
    }
}