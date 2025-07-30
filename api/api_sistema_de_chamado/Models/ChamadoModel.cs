using api_sistema_de_chamado.Enum;

namespace api_sistema_de_chamado.Models
{
    public class ChamadoModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string? Resposta { get; set; }
        public StatusChamadoEnum Status { get; set; } = StatusChamadoEnum.Pendente;
        public int UsuarioId { get; set; }
        public UsuariosModel Usuario { get; set; } = null!;
        public DateTime ChamadoDataCriacao { get; set; } = DateTime.Now;
    }
}
