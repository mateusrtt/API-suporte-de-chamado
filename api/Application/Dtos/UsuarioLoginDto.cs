using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    // DTO usado para enviar os dados de login do usuário
    public class UsuarioLoginDto
    {
        public string Email { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;
    }
}