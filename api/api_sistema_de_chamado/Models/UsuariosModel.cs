using api_sistema_de_chamado.Enum;

namespace api_sistema_de_chamado.Models
{
    public class UsuariosModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public CargoEnum Cargo { get; set; } 
        public byte[] SenhaHash { get; set; }
        public byte[] SenhaSalt { get; set; }
        public DateTime TokenDataCriacao { get; set; } = DateTime.Now;

    }
}
