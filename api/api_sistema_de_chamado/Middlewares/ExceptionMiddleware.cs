using API.Response;
using System.Net;
using System.Text.Json;

namespace api_sistema_de_chamado.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Erro de autorização/autenticação.");
                var mensagem = _env.IsDevelopment() ? ex.Message : "Acesso não autorizado.";
                await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, mensagem);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Erro de aplicação.");
                var mensagem = _env.IsDevelopment() ? ex.Message : "Erro de requisição.";
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, mensagem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado.");
                var mensagem = _env.IsDevelopment() ? ex.Message : "Ocorreu um erro inesperado.";
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, mensagem);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string mensagem)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = new Response<object>
            {
                Status = false,
                Mensagem = mensagem,
                Dados = null
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
