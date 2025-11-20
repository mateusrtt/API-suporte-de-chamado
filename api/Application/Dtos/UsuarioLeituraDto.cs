using Domain.Enums;

namespace Application.Dtos
{
    // DTO usado para retornar dados do usuário
    public class UsuarioLeituraDto
    {
        public UsuarioLeituraDto(int id, string nome, string email, CargoEnum cargo)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Cargo = cargo;
        }

        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public CargoEnum Cargo { get; set; }
    }
}
