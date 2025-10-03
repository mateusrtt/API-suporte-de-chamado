using Domain.Enums;

namespace Application.Dtos
{
    public class ChamadoLeituraDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string? Resposta { get; set; }
        public PrioridadeEnum Prioridade { get; set; }
        public StatusChamadoEnum Status { get; set; }
        public DateTime ChamadoDataCriacao { get; set; }
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
    }
}
