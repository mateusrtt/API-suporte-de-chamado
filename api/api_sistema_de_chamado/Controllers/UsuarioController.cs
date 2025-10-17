using Api.Attributes;
using Application.Dtos;
using Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Controller responsável pelos endpoints de usuários.
    /// Permite registrar, atualizar e deletar usuários, controlando acesso por perfil.
    /// </summary>
    [Route("suporte/v1/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuario _usuarioService;

        /// <summary>
        /// Construtor que injeta o serviço de usuários.
        /// </summary>
        /// <param name="usuarioService">Serviço de usuários (IUsuario) usado para processar as operações.</param>
        public UsuarioController(IUsuario usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Registra um usuário comum no sistema.
        /// </summary>
        /// <param name="dto">DTO contendo os dados do usuário a ser registrado.</param>
        /// <returns>ActionResult com o usuário criado.</returns>
        [HttpPost]
        [MensagemSucessoAtributo("Usuário cadastrado com sucesso!")]
        public async Task<ActionResult> RegistrarUsuario([FromBody] UsuarioCriacaoDto dto)
        {
            var resultado = await _usuarioService.Registrar(dto, registrarLog: true);
            return Ok(resultado);
        }

        /// <summary>
        /// Atualiza os dados de um usuário.
        /// Apenas o próprio usuário podem atualizar os dados.
        /// </summary>
        /// <param name="id">ID do usuário a ser atualizado.</param>
        /// <param name="dto">DTO contendo os dados atualizados.</param>
        /// <returns>ActionResult indicando sucesso da operação.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Usuario,Administrador")]
        [MensagemSucessoAtributo("Dados atualizados com sucesso!")]
        public async Task<ActionResult> Atualizar(int id, [FromBody] UsuarioUpdateDto dto)
        {
            var usuarioIdString = HttpContext.User.FindFirst("Id")?.Value;
            if (!int.TryParse(usuarioIdString, out var usuarioLogadoId))
                return Unauthorized();

            await _usuarioService.Atualizar(id, dto, usuarioLogadoId);

            return Ok(true);
        }

        /// <summary>
        /// Deleta a conta de um usuário.
        /// Apenas o próprio usuário podem deletar a conta.
        /// </summary>
        /// <param name="id">ID do usuário a ser deletado.</param>
        /// <returns>ActionResult indicando sucesso da operação.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Usuario,Administrador")]
        [MensagemSucessoAtributo("Conta deletada com sucesso!")]
        public async Task<ActionResult<bool>> Deletar(int id)
        {
            var usuarioIdString = HttpContext.User.FindFirst("Id")?.Value;
            if (!int.TryParse(usuarioIdString, out var usuarioLogadoId))
                return Unauthorized();

            await _usuarioService.Deletar(id, usuarioLogadoId);

            return Ok(true);
        }
    }
}