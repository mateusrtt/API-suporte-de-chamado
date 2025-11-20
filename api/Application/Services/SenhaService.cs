using Application.Interface;
using System.Security.Cryptography;

namespace Application.Services
{
    // Serviços responsável por gerar e validar senha
    public class SenhaService : ISenha
    {
        /// <summary>
        /// Gera um hash seguro e um salt para a senha informada
        /// </summary>
        /// <param name="senha">Senha em texto puro.</param>
        /// <param name="senhaHash">Hash resultante da senha.</param>
        /// <param name="senhaSalt">Salt utilizado na geração do hash.</param>
        public void CriarSenhaHash(string senha, out byte[] senhaHash, out byte[] senhaSalt)
        {

            using (var hmac = new HMACSHA512())
            {
                senhaSalt = hmac.Key;
                senhaHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
            }
        }

        /// <summary>
        /// Verifica se a senha informada corresponde ao hash e salt armazenados
        /// </summary>
        /// <param name="senha">Senha enviada pelo usuário.</param>
        /// <param name="senhaHash">Hash da senha armazenada.</param>
        /// <param name="senhaSalt">Salt usado para gerar o hash original.</param>
        /// <returns>true se a senha for válida; caso contrário, false.</returns>
        public bool VerificaSenhaHash(string senha, byte[] senhaHash, byte[] senhaSalt)
        {
            using (var hmac = new HMACSHA512(senhaSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
                return computeHash.SequenceEqual(senhaHash);
            }
        }
    }
}