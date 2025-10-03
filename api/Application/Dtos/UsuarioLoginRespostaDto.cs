namespace Application.Dtos
{
    public class UsuarioLoginRespostaDto
    {
        public UsuarioLeituraDto Usuario { get; set; }
        public string Token { get; set; } = string.Empty;

        public UsuarioLoginRespostaDto(UsuarioLeituraDto usuario, string token)
        {
            Usuario = usuario;
            Token = token;
        }
    }
}