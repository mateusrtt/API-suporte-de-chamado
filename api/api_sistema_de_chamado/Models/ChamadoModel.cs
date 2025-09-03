using api_sistema_de_chamado.Enums;

namespace api_sistema_de_chamado.Models
{
    public class ChamadoModel
    {
        public int Id { get; private set; }
        public string Descricao { get; private set; } = string.Empty;
        public string? Resposta { get; private set; }
        public PrioridadeEnum Prioridade { get; private set; }
        public StatusChamadoEnum Status { get; private set; } = StatusChamadoEnum.Pendente;
        public int UsuarioId { get; private set; }
        public UsuariosModel? Usuario { get; private set; } 
        public DateTime ChamadoDataCriacao { get; private set; } = DateTime.Now;

        // EF precisa de construtor vazio
        protected ChamadoModel() { }

        // Construtor para criação no ChamadoService → FazerChamadoAsync
        public ChamadoModel(string descricao, int usuarioId, PrioridadeEnum prioridade)
        {
            Descricao = descricao;
            UsuarioId = usuarioId;
            Prioridade = prioridade;
            Status = StatusChamadoEnum.Pendente;
            ChamadoDataCriacao = DateTime.Now;
        }

        // Método para responder chamado (ChamadoService → ResponderChamadoAsync)
        public void Responder(string resposta)
        {
            Resposta = resposta;
            Status = StatusChamadoEnum.Respondido;
        }
    }
}
