using api_sistema_de_chamado.Dtos;
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
            if (context.User.Identity?.IsAuthenticated == true && !context.Items.ContainsKey("UsuarioLogAcesso"))
            {
                context.Items["UsuarioLogAcesso"] = new UsuarioLogAcessoDto
                {
                    UsuarioId = int.Parse(context.User.FindFirst("Id")!.Value),
                    Nome = context.User.FindFirst("Username")?.Value ?? string.Empty,
                    Email = context.User.FindFirst("Email")?.Value ?? string.Empty
                };
            }

            context.Response.OnStarting(async () =>
            {
                if (context.Items["UsuarioLogAcesso"] is UsuarioLogAcessoDto usuarioLog)
                {
                    var log = new AcessoLogModel(
                        usuarioLog.UsuarioId,
                        usuarioLog.Nome,
                        usuarioLog.Email,
                        context.Connection.RemoteIpAddress?.ToString() ?? "IP desconhecido",
                        context.Request.Method,
                        context.Request.Path
                    );

                    await acessoLogRepository.AdicionarAsync(log);
                    await acessoLogRepository.SalvarAsync();
                }
            });

            await _next(context);
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