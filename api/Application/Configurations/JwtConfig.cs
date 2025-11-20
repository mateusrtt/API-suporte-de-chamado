namespace Application.Configurations
{
    // Configurações de autenticação usadas para gerar e validar o JWT
    public class JwtConfig
    {
        public string Token { get; set; } = string.Empty;
        public int ExpiracaoHoras { get; set; } = 24;
    }
}
