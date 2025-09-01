using api_sistema_de_chamado.Enum;

namespace api_sistema_de_chamado.Models
{
    public class UsuariosModel
    {
        public int Id { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public bool AceitouPoliticaPrivacidade { get; private set; }
        public CargoEnum Cargo { get; private set; } 
        public byte[] SenhaHash { get; private set; } = Array.Empty<byte>();
        public byte[] SenhaSalt { get; private set; } = Array.Empty<byte>();
        public DateTime TokenDataCriacao { get; private set; } = DateTime.Now;
        public ICollection<ChamadoModel>? Chamados { get; private set; }

        // Construtor vazio necessário para EF Core
        protected UsuariosModel() { }

        // Construtor para criação de usuários
        public UsuariosModel(string nome, string email, CargoEnum cargo, byte[] senhaHash, byte[] senhaSalt, bool aceitoupoliticaPrivacidade)
        {
            if (!aceitoupoliticaPrivacidade)
                throw new ArgumentException("É necessário aceitar a política de privacidade para se cadastrar.");

            Nome = nome;
            Email = email;
            Cargo = cargo;
            SenhaHash = senhaHash;
            SenhaSalt = senhaSalt;
            TokenDataCriacao = DateTime.Now;
            AceitouPoliticaPrivacidade = aceitoupoliticaPrivacidade;
        }
    }
}
