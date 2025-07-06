using api_sistema_de_chamado.Enum;
using System.ComponentModel.DataAnnotations;

namespace api_sistema_de_chamado.Dtos
{
    public class UsuarioCriacao
    {
        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo email é obrigatório"), EmailAddress(ErrorMessage = "Email inválido!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo senha é obrigatório")]
        public string Senha { get; set; }
        [Compare("Senha", ErrorMessage = "Senha nâo coincidem!")]
        public string ConfirmarSenha { get; set; }
        [Required(ErrorMessage = "O campo cargo é obrigatório")]
        public CargoEnum Cargo { get; set; }
    }
}
