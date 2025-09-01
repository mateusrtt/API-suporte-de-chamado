using api_sistema_de_chamado.Enum;
using System.ComponentModel.DataAnnotations;

namespace api_sistema_de_chamado.Dtos
{
    public class UsuarioCriacaoDto
    {
        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 50 caracteres")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ\s'-]+$", ErrorMessage = "O nome não pode conter números ou caracteres especiais")]
        public string Nome { get; set; } = string.Empty;
        [Required(ErrorMessage = "O campo email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido!")]
        public string Email { get; set; } = string.Empty;
        public bool AceitouPoliticaPrivacidade { get; set; }
        [Required(ErrorMessage = "O campo senha é obrigatório")]
        [StringLength(10, ErrorMessage = "A senha deve ter no máximo 10 caracteres")]
        public string Senha { get; set; } = string.Empty;
        [Compare("Senha", ErrorMessage = "Senha nâo coincidem!")]
        public string? ConfirmarSenha { get; set; } = string.Empty;
    }
}
