using Application.Dtos;

namespace Api.Extensions
{
    public static class HttpContextExtensions
    {
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