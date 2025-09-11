using Application.Interface;

namespace API.Middlewares
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