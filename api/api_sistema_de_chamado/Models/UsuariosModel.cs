using api_sistema_de_chamado.Enum;

namespace api_sistema_de_chamado.Models
{
    public class UsuariosModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public CargoEnum Cargo { get; set; } 
        public byte[] SenhaHash { get; set; } = Array.Empty<byte>();
        public byte[] SenhaSalt { get; set; } = Array.Empty<byte>();
        public DateTime TokenDataCriacao { get; set; } = DateTime.Now;
        public ICollection<ChamadoModel>? Chamados { get; set; }
    }
}
