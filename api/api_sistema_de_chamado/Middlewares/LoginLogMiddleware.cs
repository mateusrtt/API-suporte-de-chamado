using api_sistema_de_chamado.Dtos;
using api_sistema_de_chamado.Middlewares;
using api_sistema_de_chamado.Models;
using api_sistema_de_chamado.Repositories.AcessoLog;

namespace api_sistema_de_chamado.Middlewares
{
    public class LoginLogMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAcessoLogRepository acessoLogRepository)
        {
            await _next(context);

            // Só processa logs se for o endpoint de login
            if (context.Request.Path.Value!.Equals("/suporte/v1/usuarios/login", StringComparison.OrdinalIgnoreCase)
                && context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase)
                && context.Items.ContainsKey("LoginUsuario")
                && context.Items["LoginUsuario"] is LoginResultadoDto usuarioLog)
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

                    context.Items["LoginUsuarioLogRegistrado"] = true;
                }
            }
        }
    }

    // Extensão para registrar o middleware
    public static class LoginLogMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoginLogs(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoginLogMiddleware>();
        }
    }