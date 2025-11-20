namespace Application.Dtos
{
    // DTO usado para cadastrar um novo usuário
    public class UsuarioCriacaoDto
    {
        public string Nome { get; set; } = string.Empty;
       
        public string Email { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;
        
        public string? ConfirmarSenha { get; set; } = string.Empty;

        public bool AceitouPoliticaPrivacidade { get; set; }
    }
}