using Application.Dtos;

namespace Api.Extensions
{
    /// <summary>
    /// Extensão para HttpContext que facilita a extração de informações do usuário autenticado.
    /// Permite recuperar o ID, nome e e-mail do usuário a partir do contexto HTTP ou de DTOs específicos.
    /// </summary>
    /// <remarks>
    /// - ExtrairUsuario: retorna dados básicos do usuário a partir do HttpContext.
    /// - ExtrairUsuarioDeDto: retorna dados equivalentes a partir de objetos DTO conhecidos (UsuarioLoginRespostaDto, UsuarioLeituraDto).
    /// - Caso o usuário não esteja autenticado ou o DTO não seja reconhecido, valores padrão são retornados.
    /// </remarks>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Extrai informações do usuário a partir do HttpContext.
        /// </summary>
        /// <param name="context">O HttpContext da requisição atual.</param>
        /// <returns>Tupla contendo Id (nullable), Nome e Email do usuário.</returns>
        public static (int? Id, string Nome, string Email) ExtrairUsuario(this HttpContext context)
        {
            int? usuarioId = null;
            string nome = "Desconhecido";
            string email = "Não fornecido";

            if (context.User.Identity?.IsAuthenticated == true)
            {
                usuarioId = int.TryParse(context.User.FindFirst("Id")?.Value, out var uid) ? uid : null;
                nome = context.User.FindFirst("Username")?.Value ?? nome;
                email = context.User.FindFirst("Email")?.Value ?? email;
            }

            return (usuarioId, nome, email);
        }

        /// <summary>
        /// Extrai informações do usuário a partir de um DTO específico.
        /// </summary>
        /// <param name="dto">Objeto DTO que contém informações do usuário.</param>
        /// <returns>Tupla contendo Id (nullable), Nome e Email do usuário.</returns>
        public static (int? Id, string Nome, string Email) ExtrairUsuarioDeDto(this object dto)
        {
            return dto switch
            {
                UsuarioLoginRespostaDto login => (login.Usuario.Id, login.Usuario.Nome, login.Usuario.Email),
                UsuarioLeituraDto registro => (registro.Id, registro.Nome, registro.Email),
                _ => (null, "Desconhecido", "Não fornecido")
            };
        }
    }
}