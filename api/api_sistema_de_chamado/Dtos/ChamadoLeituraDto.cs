using api_sistema_de_chamado.Enum;

namespace api_sistema_de_chamado.Dtos
{
    public class ChamadoLeituraDto
    {
        public int Id { get; set; }
        public string Local { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string? Resposta { get; set; }
        public StatusChamadoEnum Status { get; set; }
        public DateTime ChamadoDataCriacao { get; set; }
        public int UsuarioId { get; set; }
        public string? NomeUsuario { get; set; }
    }
}
