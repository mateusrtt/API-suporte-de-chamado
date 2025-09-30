using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Application.Interface;
using Api.Extensions;

/// <summary>
/// Filtro de ação que registra logs de acesso dos usuários.
/// Captura informações como ID, nome, e-mail, IP, método HTTP e URL do endpoint.
/// Para DELETE, registra antes da execução; para outros métodos, registra após a execução, se não houver exceção.
/// </summary>
public class FiltroLogAcesso : IAsyncActionFilter
{
    private readonly IAcessoLog _logService;
    
    public FiltroLogAcesso(IAcessoLog logService)
    {
        _logService = logService;
    }

    /// <summary>
    /// Intercepta a execução da ação para registrar logs de acesso.
    /// </summary>
    /// <param name="contexto">Contexto da execução da ação contendo informações da requisição.</param>
    /// <param name="next">Delegate para continuar a execução do pipeline.</param>
    public async Task OnActionExecutionAsync(ActionExecutingContext contexto, ActionExecutionDelegate next)
    {
        bool Delete = contexto.HttpContext.Request.Method == HttpMethods.Delete;

        var (usuarioId, nome, email) = contexto.HttpContext.ExtrairUsuario();
        string metodoHttp = contexto.HttpContext.Request.Method;
        string urlEndpoint = $"{contexto.HttpContext.Request.Path}{contexto.HttpContext.Request.QueryString}";
        string ipCliente = contexto.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "IP desconhecido";

        if (Delete)
            await _logService.Registrar(usuarioId, nome, email, ipCliente, metodoHttp, urlEndpoint);

        var contextoExecutado = await next();

        if (!Delete && contextoExecutado.Exception == null)
        {
            if (!contexto.HttpContext.User.Identity?.IsAuthenticated ?? true)
            {
                if (contextoExecutado.Result is ObjectResult resultadoObj && resultadoObj.Value != null)
                {
                    (usuarioId, nome, email) = resultadoObj.Value.ExtrairUsuarioDeDto();
                }
            }
            await _logService.Registrar(usuarioId, nome, email, ipCliente, metodoHttp, urlEndpoint);
        }
    }
}