using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Application.Dtos;
using Domain.Interfaces;
using Domain.Models;

public class FiltroLogAcesso : IAsyncActionFilter
{
    private readonly IAcessoLogRepository _logRepo;
    private readonly ILogger<FiltroLogAcesso> _logger;

    public FiltroLogAcesso(IAcessoLogRepository logRepo, ILogger<FiltroLogAcesso> logger)
    {
        _logRepo = logRepo;
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext contexto, ActionExecutionDelegate next)
    {
        bool ehDelete = contexto.HttpContext.Request.Method == HttpMethods.Delete;

        // Dados iniciais do usuário (se estiver autenticado)
        int? usuarioId = null;
        string nome = "Desconhecido";
        string email = "Não fornecido";

        if (contexto.HttpContext.User.Identity?.IsAuthenticated == true)
        {
            usuarioId = int.TryParse(contexto.HttpContext.User.FindFirst("Id")?.Value, out var uid) ? uid : null;
            nome = contexto.HttpContext.User.FindFirst("Username")?.Value ?? nome;
            email = contexto.HttpContext.User.FindFirst("Email")?.Value ?? email;
        }

        string metodoHttp = contexto.HttpContext.Request.Method;
        string urlEndpoint = $"{contexto.HttpContext.Request.Path}{contexto.HttpContext.Request.QueryString}";
        string ipCliente = contexto.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "IP desconhecido";

        async Task RegistrarLogAsync()
        {
            try
            {
                var log = new AcessoLog(usuarioId, nome, email, ipCliente, metodoHttp, urlEndpoint);
                await _logRepo.AdicionarAsync(log);
                await _logRepo.SalvarAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao registrar log de acesso");
            }
        }

        // Garante registro antes do DELETE
        if (ehDelete)
            await RegistrarLogAsync();

        // Executa a ação
        var contextoExecutado = await next();

        // Registrar log após execução para outras ações, se não houver exceção
        if (!ehDelete && contextoExecutado.Exception == null)
        {
            // Se não estiver autenticado, pode ser login ou registro
            if (!contexto.HttpContext.User.Identity?.IsAuthenticated ?? true)
            {
                if (contextoExecutado.Result is ObjectResult resultadoObj)
                {
                    if (resultadoObj.Value != null)
                    {
                        (usuarioId, nome, email) = ExtrairInfoUsuario(resultadoObj.Value);
                    }
                }
            }

            await RegistrarLogAsync();
        }
    }

    // Helper que unifica extração de Id/Nome/Email de diferentes DTOs
    private static (int? Id, string Nome, string Email) ExtrairInfoUsuario(object dto)
    {
        return dto switch
        {
            UsuarioLoginRespostaDto login => (login.Usuario.Id, login.Usuario.Nome, login.Usuario.Email),
            UsuarioLeituraDto registro => (registro.Id, registro.Nome, registro.Email),
            _ => (null, "Desconhecido", "Não fornecido")
        };
    }
}