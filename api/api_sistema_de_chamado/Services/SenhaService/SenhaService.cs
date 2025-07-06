using System.Security.Cryptography;

namespace api_sistema_de_chamado.Services.SenhaService
{
    public class SenhaService : ISenhaInterface
    {
        public void CriarSenhaHash(string senha, out byte[] senhaHash, out byte[] senhaSalt)
        {

            using (var hmac = new HMACSHA512())
            {
                senhaSalt = hmac.Key;
                senhaHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
            }
        }

    }
}
