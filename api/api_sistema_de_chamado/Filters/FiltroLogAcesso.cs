using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Application.Interface;
using Api.Extensions;

public class FiltroLogAcesso : IAsyncActionFilter
{
    private readonly IAcessoLog _logService;
    
    public FiltroLogAcesso(IAcessoLog logService)
    {
        _logService = logService;
    }

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