using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class FazerChamadoDto
    {
        [Required(ErrorMessage = "A descrição é obrigatória")]
        public string Descricao { get; set; } = string.Empty;
    }
}
