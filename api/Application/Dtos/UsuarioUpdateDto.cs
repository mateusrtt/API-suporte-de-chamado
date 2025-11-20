namespace Application.Dtos
{
    // DTO usado para atualizar dados do usuário
    public class UsuarioUpdateDto
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? SenhaAtual { get; set; }
        public string? NovaSenha { get; set; }
        public string? ConfirmarNovaSenha { get; set; }
    }
}