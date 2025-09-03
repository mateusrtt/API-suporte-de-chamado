using api_sistema_de_chamado.Enums;
using System.ComponentModel.DataAnnotations;

namespace api_sistema_de_chamado.Dtos
{
    public class UsuarioCriacaoDto
    {
        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo email é obrigatório")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo senha é obrigatório")]
        public string Senha { get; set; } = string.Empty;
        
        public string? ConfirmarSenha { get; set; } = string.Empty;

        public bool AceitouPoliticaPrivacidade { get; set; }
    }
}
