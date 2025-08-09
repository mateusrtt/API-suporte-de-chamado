using System.ComponentModel.DataAnnotations;

namespace api_sistema_de_chamado.Dtos
{
    public class FazerChamadoDto
    {
        [Required(ErrorMessage = "A descrição é obrigatória")]
        public string Descricao { get; set; } = string.Empty;
    }
}
