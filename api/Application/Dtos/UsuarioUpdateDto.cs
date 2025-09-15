namespace Application.Dtos
{
    public class UsuarioUpdateDto
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? SenhaAtual { get; set; }
        public string? NovaSenha { get; set; }
        public string? ConfirmarNovaSenha { get; set; }
    }
}