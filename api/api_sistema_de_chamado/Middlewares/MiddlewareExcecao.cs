using Api.Response;
using Domain.Exceptions;
using Infrastructure.Exceptions;
using System.Net;
using System.Text.Json;

namespace Api.Middlewares
{
    public class MiddlewareExcecao
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MiddlewareExcecao> _logger;
        private readonly IHostEnvironment _env;

        public MiddlewareExcecao(RequestDelegate next, ILogger<MiddlewareExcecao> logger, IHostEnvironment env)
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
            catch (ExcecaoDominio ex)
            {
                _logger.LogWarning(ex, "Erro controlado.");
                var mensagem = ex.Message;
                await HandleExceptionAsync(context, ex.StatusCode, mensagem);
            }
            catch (ExcecaoInfraestrutura ex) 
            {
                _logger.LogError(ex, "Erro de infraestrutura.");
                var mensagem = _env.IsDevelopment() ? ex.Message : "Ocorreu um erro no servidor."; 
                await HandleExceptionAsync(context, ex.StatusCode, mensagem);
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
                Dados = null,
                Mensagem = mensagem,
                Status = false,
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }
}