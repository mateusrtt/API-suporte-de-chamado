using Application.Dtos;
using Application.Interface;

namespace Api.Middlewares
{
    public class AcessoLogMiddleware
    {
        private readonly RequestDelegate _next;

        public AcessoLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAcessoLog acessoLogService)
        {
            // Preenche antes do controller, se houver JWT
            if (context.User.Identity?.IsAuthenticated == true &&
                !context.Items.ContainsKey("UsuarioLogAcesso"))
            {
                context.Items["UsuarioLogAcesso"] = new UsuarioLogAcessoDto
                {
                    UsuarioId = int.Parse(context.User.FindFirst("Id")!.Value),
                    Nome = context.User.FindFirst("Username")?.Value ?? string.Empty,
                    Email = context.User.FindFirst("Email")?.Value ?? string.Empty
                };
            }

            // Controller executa com HttpContext.Items preenchido
            await _next(context);

            // Depois registra o log
            await acessoLogService.RegistrarLog();
        }
    }

    public static class AcessoLogMiddlewareExtensions
        {
            public static IApplicationBuilder UseAcessoLogs(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<AcessoLogMiddleware>();
            }
        }
    }