using Domain.Enums;

namespace Domain.Models
{
    public class Chamado
    {
        public int Id { get; private set; }
        public string Descricao { get; private set; } = string.Empty;
        public string? Resposta { get; private set; }
        public PrioridadeEnum Prioridade { get; private set; }
        public StatusChamadoEnum Status { get; private set; } = StatusChamadoEnum.Pendente;
        public int UsuarioId { get; private set; }
        public Usuario? Usuario { get; private set; } 
        public DateTime ChamadoDataCriacao { get; private set; } = DateTime.Now;

        // EF precisa de construtor vazio
        protected Chamado() { }

        public Chamado(string descricao, int usuarioId, PrioridadeEnum prioridade)
        {
            Descricao = descricao;
            UsuarioId = usuarioId;
            Prioridade = prioridade;
            Status = StatusChamadoEnum.Pendente;
            ChamadoDataCriacao = DateTime.Now;
        }

        public void Responder(string resposta)
        {
            Resposta = resposta;
            Status = StatusChamadoEnum.Respondido;
        }
    }
}