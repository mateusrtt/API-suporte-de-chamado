using api_sistema_de_chamado.Data;
using api_sistema_de_chamado.Dtos;
using api_sistema_de_chamado.Models;
using api_sistema_de_chamado.Services.SenhaService;

namespace api_sistema_de_chamado.Services.AuthService
{
    public class AuthService : IAuthInterface
    {
        private readonly AppDbContext _context;
        private readonly ISenhaInterface _senhaInterface;

        public AuthService(AppDbContext context, ISenhaInterface senhaInterface)
        {
            _context = context;
            _senhaInterface = senhaInterface;
        }

        public async Task<Response<UsuarioCriacaoDto>> Registrar(UsuarioCriacaoDto usuarioRegistro)
        {
            Response<UsuarioCriacaoDto> respostaServico = new Response<UsuarioCriacaoDto>();

            try
            {
                if (!VerificaSeEmailUsuarioJaExiste(usuarioRegistro))
                {
                    respostaServico.Dados = null;
                    respostaServico.Status = false;
                    respostaServico.Mensagem = "Email/usuário já cadastrados!";
                    return respostaServico;
                }

                _senhaInterface.CriarSenhaHash(usuarioRegistro.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                UsuariosModel usuario = new UsuariosModel()
                {
                    Nome = usuarioRegistro.Nome,
                    Email = usuarioRegistro.Email,
                    Cargo = usuarioRegistro.Cargo,
                    SenhaHash = senhaHash,
                    SenhaSalt = senhaSalt
                };

                _context.Add(usuario);
                await _context.SaveChangesAsync();

                respostaServico.Mensagem = "Usuário cadastrado com sucesso!";

            }
            catch (Exception ex)
            {
                respostaServico.Dados = null;
                respostaServico.Mensagem = ex.Message;
                respostaServico.Status = false;
            }

            return respostaServico;
        }


        public bool VerificaSeEmailUsuarioJaExiste(UsuarioCriacaoDto usuarioRegistro)
        {
            var usuario = _context.Usuario.FirstOrDefault(userBanco => userBanco.Email == usuarioRegistro.Email || userBanco.Nome == usuarioRegistro.Nome);

            if (usuario != null) return false;

            return true;

        }


    }
}