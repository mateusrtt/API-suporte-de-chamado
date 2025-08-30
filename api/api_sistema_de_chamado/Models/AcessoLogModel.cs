namespace api_sistema_de_chamado.Models
{
    public class AcessoLogModel
    {
        public int Id { get; private set; }
        public int? UsuarioId { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty; 
        public string IpAcesso { get; private set; } = string.Empty;
        public string MetodoHttp { get; private set; } = string.Empty;
        public string Endpoint { get; private set; } = string.Empty;
        public DateTime DataHora { get; private set; } = DateTime.Now;

        // Construtor vazio EF
        protected AcessoLogModel() { }

        public AcessoLogModel(int? usuarioId, string nome, string email, string ip, string metodo, string endpoint)
        {
            UsuarioId = usuarioId;
            Nome = nome;
            Email = email;
            IpAcesso = ip;
            MetodoHttp = metodo;
            Endpoint = endpoint;
            DataHora = DateTime.Now;
        }
    }
}
