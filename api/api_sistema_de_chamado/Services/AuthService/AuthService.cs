using api_sistema_de_chamado.Data;
using api_sistema_de_chamado.Dtos;
using api_sistema_de_chamado.Models;
using api_sistema_de_chamado.Services.SenhaService;
using api_sistema_de_chamado.Repositories.Usuario;
using api_sistema_de_chamado.Enum;

namespace api_sistema_de_chamado.Services.AuthService
{
    public class AuthService : IAuthInterface
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ISenhaInterface _senhaInterface;

        public AuthService(IUsuarioRepository usuarioRepository, ISenhaInterface senhaInterface)
        {
            _usuarioRepository = usuarioRepository;
            _senhaInterface = senhaInterface;
        }


        public async Task<Response<UsuarioCriacaoDto>> RegistrarUsuario(UsuarioCriacaoDto usuarioRegistro)
        {
            return await Registrar(usuarioRegistro, CargoEnum.Usuario);
        }

        public async Task<Response<UsuarioCriacaoDto>> RegistrarAdmin(UsuarioCriacaoDto usuarioRegistro)
        {
            return await Registrar(usuarioRegistro, CargoEnum.Administrador);
        }


        public async Task<Response<UsuarioCriacaoDto>> Registrar(UsuarioCriacaoDto usuarioRegistro, CargoEnum cargo)
        {
            Response<UsuarioCriacaoDto> respostaServico = new Response<UsuarioCriacaoDto>();

            try
            {
                bool usuarioDisponivel = await _usuarioRepository.ExisteUsuarioOuEmailAsync(
                     usuarioRegistro.Nome, usuarioRegistro.Email);


                if (!usuarioDisponivel)
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
                    Cargo = cargo,
                    SenhaHash = senhaHash,
                    SenhaSalt = senhaSalt
                };

                await _usuarioRepository.AdicionarAsync(usuario);
                await _usuarioRepository.SalvarAsync();

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

        public async Task<Response<String>> Login(UsuarioLoginDto usuarioLogin) 
        {
            Response<String> respostaServico = new Response<String>();

            try
            {
                var usuario = await _usuarioRepository.ObterPorEmailAsync(usuarioLogin.Email);

                if (usuario == null)
                {
                    respostaServico.Mensagem = "Email invalido!";
                    respostaServico.Status = false;
                    return respostaServico;
                }

                if (!_senhaInterface.VerificaSenhaHash(usuarioLogin.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                {
                    respostaServico.Mensagem = "Senha invalido!";
                    respostaServico.Status = false;
                    return respostaServico;
                }

                var token = _senhaInterface.CriarToken(usuario);

                respostaServico.Dados = token;
                respostaServico.Mensagem = "Usuário logado com sucesso!";

            } catch (Exception ex)
            {
                respostaServico.Dados = null;
                respostaServico.Mensagem = ex.Message;
                respostaServico.Status = false;
            }
                
            return respostaServico;
        }

    }
}