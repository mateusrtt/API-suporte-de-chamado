using Api.Response;
using Domain.Exceptions;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Middlewares
{
    public class MiddlewareExcecao
    {
        /// <summary>
        /// Middleware responsável por capturar exceções durante o processamento das requisições HTTP.
        /// Retorna respostas JSON padronizadas com mensagens adequadas dependendo do tipo de exceção.
        /// </summary>
        private readonly RequestDelegate _next;
        private readonly ILogger<MiddlewareExcecao> _logger;
        private readonly IHostEnvironment _env;

        /// <summary>
        /// Construtor do MiddlewareExcecao.
        /// </summary>
        public MiddlewareExcecao(RequestDelegate next, ILogger<MiddlewareExcecao> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Intercepta a execução da requisição, capturando exceções e tratando-as de forma padronizada.
        /// </summary>
        /// <param name="context">Contexto HTTP da requisição atual.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ExcecaoAplicacao ex)
            {
                _logger.LogWarning(ex, "Erro controlado.");
                var mensagem = _env.IsDevelopment() ? ex.Message : "Ocorreu um erro na requisição.";
                await HandleExceptionAsync(context, ex.StatusCode, mensagem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado.");
                var mensagem = _env.IsDevelopment() ? ex.Message : "Ocorreu um erro inesperado.";
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, mensagem);
            }
        }

        /// <summary>
        /// Envia a resposta JSON padronizada para o cliente.
        /// </summary>
        /// <param name="context">Contexto HTTP da requisição atual.</param>
        /// <param name="statusCode">Código de status HTTP a ser retornado.</param>
        /// <param name="mensagem">Mensagem de erro a ser incluída na resposta.</param>
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