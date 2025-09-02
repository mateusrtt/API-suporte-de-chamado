using api_sistema_de_chamado.Enum;
using api_sistema_de_chamado.Services.SenhaService;
using System.Text.RegularExpressions;

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

        public UsuariosModel(string nome, string email, CargoEnum cargo, byte[] senhaHash, byte[] senhaSalt, bool aceitoupoliticaPrivacidade)
        {
            if (!Regex.IsMatch(nome, @"^[A-Za-zÀ-ÿ\s'-]{3,60}$"))
                throw new ArgumentException("Nome inválido: deve ter entre 3 e 60 caracteres e não conter números ou símbolos.");

            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]{3,}@[a-zA-Z0-9.-]{3,}\.[a-zA-Z]{3,}$"))
                throw new ArgumentException("Formato de email inválido");

            if (senhaHash == null || senhaSalt == null)
                throw new ArgumentException("Falha ao processar a senha");

            if (!aceitoupoliticaPrivacidade)
                throw new ArgumentException("É necessário aceitar a política de privacidade para se cadastrar");

            Nome = nome;
            Email = email.ToLower();
            Cargo = cargo;
            SenhaHash = senhaHash;
            SenhaSalt = senhaSalt;
            TokenDataCriacao = DateTime.Now;
            AceitouPoliticaPrivacidade = aceitoupoliticaPrivacidade;
        }

        public void AlterarSenha(string novaSenha, ISenhaInterface senhaInterface)
        {
            if (!Regex.IsMatch(novaSenha, @"^.{5,10}$"))
                throw new ArgumentException("Senha inválida: deve ter entre 5 e 10 caracteres");

            senhaInterface.CriarSenhaHash(novaSenha, out byte[] hash, out byte[] salt);
            SenhaHash = hash;
            SenhaSalt = salt;
        }

        public void ValidarSenha(string senha, ISenhaInterface senhaInterface)
        {
            if (!senhaInterface.VerificaSenhaHash(senha, SenhaHash, SenhaSalt))
                throw new ApplicationException("Senha inválida!");
        }

        public void RevogarAceitePolitica()
        {
            AceitouPoliticaPrivacidade = false;
        }
    }
}