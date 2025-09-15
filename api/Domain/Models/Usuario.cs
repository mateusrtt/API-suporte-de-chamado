using Domain.Enums;
using System.Text.RegularExpressions;

namespace Domain.Models
{
    public class Usuario
    {
        public int Id { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public bool AceitouPoliticaPrivacidade { get; private set; }
        public CargoEnum Cargo { get; private set; } 
        public byte[] SenhaHash { get; private set; } = Array.Empty<byte>();
        public byte[] SenhaSalt { get; private set; } = Array.Empty<byte>();
        public DateTime TokenDataCriacao { get; private set; } = DateTime.Now;
        public ICollection<Chamado>? Chamados { get; private set; }

        // Construtor vazio necessário para EF Core
        protected Usuario() { }

        public Usuario(string nome, string email, CargoEnum cargo, byte[] senhaHash, byte[] senhaSalt, bool aceitoupoliticaPrivacidade)
        {
            DefinirNome(nome);
            DefinirEmail(email);

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

        public void DefinirNome(string nome)
        {
            if (!Regex.IsMatch(nome, @"^[A-Za-zÀ-ÿ\s'-]{3,60}$"))
                throw new ArgumentException("Nome inválido: deve ter entre 3 e 60 caracteres e não conter números ou símbolos.");
            Nome = nome;
        }

        public void DefinirEmail(string email)
        {
            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]{3,}@[a-zA-Z0-9.-]{3,}\.[a-zA-Z]{3,}$"))
                throw new ArgumentException("Formato de email inválido");
            Email = email.ToLower();
        }

        public void DefinirSenha(byte[] novaSenhaHash, byte[] novaSenhaSalt)
        {
            if (novaSenhaHash == null || novaSenhaSalt == null)
                throw new ArgumentException("Falha ao processar a senha.");

            SenhaHash = novaSenhaHash;
            SenhaSalt = novaSenhaSalt;
        }

        public void RevogarAceitePolitica()
        {
            AceitouPoliticaPrivacidade = false;
        }
    }
}