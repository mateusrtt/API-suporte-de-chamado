using System.ComponentModel.DataAnnotations;

namespace api_sistema_de_chamado.Dtos
{
    public class FazerChamadoDto
    {
        [Required(ErrorMessage = "O campo Local é obrigatório")]
        public string Local { get; set; } = string.Empty;
        [Required(ErrorMessage = "A descrição é obrigatória")]
        public string Descricao { get; set; } = string.Empty;
    }
}
