using api_sistema_de_chamado.Dtos;
using api_sistema_de_chamado.Models;
using api_sistema_de_chamado.Repositories.AcessoLog;
using api_sistema_de_chamado.Services.AcessoLogService;

namespace api_sistema_de_chamado.Middlewares
{
    public class AcessoLogMiddleware
    {
        private readonly RequestDelegate _next;

        public AcessoLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAcessoLogService acessoLogService)
        {
            context.Response.OnStarting(async state =>
            {
                await acessoLogService.RegistrarLog();
            }, default!);

            await _next(context);
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