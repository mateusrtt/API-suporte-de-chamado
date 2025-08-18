using api_sistema_de_chamado.Enum;
using System.ComponentModel.DataAnnotations;

namespace api_sistema_de_chamado.Dtos
{
    public class ResponderChamadoDto
    {
        [Required(ErrorMessage = "A resposta é obrigatória")]
        public string Resposta { get; set; } = string.Empty;
    }
}
