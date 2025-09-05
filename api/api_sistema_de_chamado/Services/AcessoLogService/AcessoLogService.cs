using api_sistema_de_chamado.Dtos;
using api_sistema_de_chamado.Models;
using api_sistema_de_chamado.Repositories.AcessoLog;

namespace api_sistema_de_chamado.Services.AcessoLogService
{
    public class AcessoLogService : IAcessoLogService
    {
        private readonly IAcessoLogRepository _acessoLogRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AcessoLogService(IAcessoLogRepository acessoLogRepository, IHttpContextAccessor httpContextAccessor)
        {
            _acessoLogRepository = acessoLogRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task RegistrarLog()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null) return;

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

            if (context.Items["UsuarioLogAcesso"] is UsuarioLogAcessoDto usuarioLog)
            {
                var log = new AcessoLogModel(
                    usuarioLog.UsuarioId,
                    usuarioLog.Nome,
                    usuarioLog.Email,
                    context.Connection.RemoteIpAddress?.ToString() ?? "IP desconhecido",
                    context.Request.Method,
                    context.Request.Path
                );

                await _acessoLogRepository.AdicionarAsync(log);
                await _acessoLogRepository.SalvarAsync();
            }
        }
    }
}