namespace Application.Configurations
{
    public class JwtConfig
    {
        public string Token { get; set; } = string.Empty;
        public int ExpiracaoHoras { get; set; } = 24;
    }
}
