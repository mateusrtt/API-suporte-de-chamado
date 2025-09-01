using api_sistema_de_chamado.Models;
using api_sistema_de_chamado.Repositories.AcessoLog;

namespace api_sistema_de_chamado.Middlewares
{
    public class AcessoLogMiddleware
    {
        private readonly RequestDelegate _next;

        public AcessoLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAcessoLogRepository acessoLogRepository)
        {
            await _next(context);

            try
            {
                // Ignora rotas internas
                var path = context.Request.Path.ToString().ToLower();
                if (path.StartsWith("/swagger") ||
                    path.StartsWith("/health") ||
                    path.StartsWith("/metrics") ||
                    path.Equals("/suporte/v1/usuarios/login") ||
                    (path.Equals("/suporte/v1/usuarios") && context.Request.Method == "POST") ||   
                    (path.Equals("/suporte/v1/usuarios/admins") && context.Request.Method == "POST")) 
                    return;

                int? usuarioId = context.User.Identity?.IsAuthenticated == true? int.Parse(context.User.FindFirst("Id")!.Value): null;
                string nome = context.User.FindFirst("Username")?.Value ?? string.Empty; 
                string email = context.User.FindFirst("Email")?.Value ?? string.Empty;

                // Cria o log
                var log = new AcessoLogModel(
                    usuarioId,
                    nome,
                    email,
                    context.Connection.RemoteIpAddress?.ToString() ?? "IP desconhecido",
                    context.Request.Method,
                    context.Request.Path
                );

                // Adiciona e salva usando o repository
                await acessoLogRepository.AdicionarAsync(log);
                await acessoLogRepository.SalvarAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar log: {ex.Message}");
            }
        }
    }

    // Extensão para registrar o middleware
    public static class AcessoLogMiddlewareExtensions
    {
        public static IApplicationBuilder UseAcessoLogs(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AcessoLogMiddleware>();
        }
    }
}
