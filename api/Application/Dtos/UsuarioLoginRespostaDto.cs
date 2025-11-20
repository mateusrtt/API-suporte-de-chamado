namespace Application.Dtos
{
    // DTO usado para retornar o usuário e o token após login
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