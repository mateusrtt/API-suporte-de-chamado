using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class ResponderChamadoDto
    {
        [Required(ErrorMessage = "A resposta é obrigatória")]
        public string Resposta { get; set; } = string.Empty;
    }
}
