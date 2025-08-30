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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IUsuarioRepository usuarioRepository, ISenhaInterface senhaInterface, IHttpContextAccessor httpContextAccessor)
        {
            _usuarioRepository = usuarioRepository;
            _senhaInterface = senhaInterface;
            _httpContextAccessor = httpContextAccessor;
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
                bool usuarioDisponivel = await _usuarioRepository.ExisteUsuarioOuEmailAsync(
                     usuarioRegistro.Nome, usuarioRegistro.Email);

                if (!usuarioDisponivel)
                    throw new ApplicationException("Email/usuário já cadastrados!");

                _senhaInterface.CriarSenhaHash(usuarioRegistro.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                UsuariosModel usuario = new UsuariosModel(
                    usuarioRegistro.Nome,
                    usuarioRegistro.Email,
                    cargo,
                    senhaHash,
                    senhaSalt
                );

                await _usuarioRepository.AdicionarAsync(usuario);
                await _usuarioRepository.SalvarAsync();

                return usuarioRegistro;
        }

        public async Task<String> Login(UsuarioLoginDto usuarioLogin) 
        {
         var usuario = await _usuarioRepository.ObterPorEmailAsync(usuarioLogin.Email);

         if (usuario == null)
            throw new ApplicationException("Email inválido!");

         if (!_senhaInterface.VerificaSenhaHash(usuarioLogin.Senha, usuario.SenhaHash, usuario.SenhaSalt))
            throw new ApplicationException("Senha inválida!");

            var token = _senhaInterface.CriarToken(usuario);

            _httpContextAccessor.HttpContext!.Items["LoginUsuario"] = new LoginResultadoDto
            {
                UsuarioId = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };
            return token;
        }
    }
}