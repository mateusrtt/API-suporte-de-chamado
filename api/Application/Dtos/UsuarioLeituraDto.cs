namespace Application.Dtos
{
    public class UsuarioLeituraDto
    {
        public UsuarioLeituraDto(int id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
        }

        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
