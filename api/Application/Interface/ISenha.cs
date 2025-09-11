using Domain.Models;

namespace Application.Interface
{
    public interface ISenha
    {
        void CriarSenhaHash(string senha, out byte[] senhaHash, out byte[] senhaSalt);
        bool VerificaSenhaHash(string senha, byte[] senhaHash, byte[] senhaSalt);
        string CriarToken(Usuario usuario);
    }
}
