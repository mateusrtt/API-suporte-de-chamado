using Application.Interface;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class AcessoLogService : IAcessoLog
    {
        private readonly IAcessoLogRepository _logRepo;
        private readonly ILogger<AcessoLogService> _logger;

        public AcessoLogService(IAcessoLogRepository logRepo, ILogger<AcessoLogService> logger)
        {
            _logRepo = logRepo;
            _logger = logger;
        }

        public async Task Registrar(int? usuarioId, string nome, string email, string ip, string metodo, string url)
        {
            try
            {
                var log = new AcessoLog(usuarioId, nome, email, ip, metodo, url);
                await _logRepo.AdicionarAsync(log);
                await _logRepo.SalvarAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao registrar log de acesso");
            }
        }
    }
}