using api_sistema_de_chamado.Data;
using api_sistema_de_chamado.Dtos;
using api_sistema_de_chamado.Models;
using api_sistema_de_chamado.Services.SenhaService;
using api_sistema_de_chamado.Repositories.Usuario;
using api_sistema_de_chamado.Enum;
using api_sistema_de_chamado.Repositories.AcessoLog;
using System.Text.RegularExpressions;

namespace api_sistema_de_chamado.Services.AuthService
{
    public class AuthService : IAuthInterface
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ISenhaInterface _senhaInterface;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAcessoLogRepository _acessoLogRepository;

        public AuthService(IUsuarioRepository usuarioRepository, ISenhaInterface senhaInterface, IHttpContextAccessor httpContextAccessor, IAcessoLogRepository acessoLogRepository)
        {
            _usuarioRepository = usuarioRepository;
            _senhaInterface = senhaInterface;
            _httpContextAccessor = httpContextAccessor;
            _acessoLogRepository = acessoLogRepository;
        }


        public async Task<UsuarioCriacaoDto> RegistrarUsuario(UsuarioCriacaoDto usuarioRegistro)
        {
            return await Registrar(usuarioRegistro, CargoEnum.Usuario);
        }

        public async Task<UsuarioCriacaoDto> RegistrarAdmin(UsuarioCriacaoDto usuarioRegistro)
        {
            return await Registrar(usuarioRegistro, CargoEnum.Administrador);
        }

        public async Task<UsuarioCriacaoDto> Registrar(UsuarioCriacaoDto usuarioRegistro, CargoEnum cargo)
        {
            var usuarioExistente = await _usuarioRepository.ObterPorEmailAsync(usuarioRegistro.Email.ToLower());
            if (usuarioExistente != null)
                throw new ApplicationException("Email já cadastrado.");

            if (string.IsNullOrWhiteSpace(usuarioRegistro.Senha) || !Regex.IsMatch(usuarioRegistro.Senha, @"^.{5,10}$"))
                throw new ApplicationException("Senha inválida: deve ter entre 5 e 10 caracteres");

            if (usuarioRegistro.Senha != usuarioRegistro.ConfirmarSenha)
                throw new ApplicationException("Senha e confirmação não coincidem");

            _senhaInterface.CriarSenhaHash(usuarioRegistro.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                UsuariosModel usuario = new UsuariosModel(
                    usuarioRegistro.Nome,
                    usuarioRegistro.Email.ToLower(),
                    cargo,
                    senhaHash,
                    senhaSalt,
                    usuarioRegistro.AceitouPoliticaPrivacidade
                );

                await _usuarioRepository.AdicionarAsync(usuario);
                await _usuarioRepository.SalvarAsync();

                _httpContextAccessor.HttpContext!.Items["UsuarioLogAcesso"] = new UsuarioLogAcessoDto
                {
                    UsuarioId = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email
                };

            return new UsuarioCriacaoDto();
        }

        public async Task<String> Login(UsuarioLoginDto usuarioLogin) 
        {

         var usuario = await _usuarioRepository.ObterPorEmailAsync(usuarioLogin.Email.ToLower());
         if (usuario == null)
            throw new ApplicationException("Email inválido!");

            usuario.ValidarSenha(usuarioLogin.Senha, _senhaInterface);

            var token = _senhaInterface.CriarToken(usuario);

            _httpContextAccessor.HttpContext!.Items["UsuarioLogAcesso"] = new UsuarioLogAcessoDto
            {
                UsuarioId = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };
            return token;
        }
    }
}