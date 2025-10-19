using Application.Interface;
using Domain.Interfaces;
using Application.Dtos;
using Domain.Exceptions;

namespace Application.Services
{
    public class AuthService : IAuth
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ISenha _senhaInterface;
        private readonly IToken _tokenService;

        public AuthService(IUsuarioRepository usuarioRepository, ISenha senhaInterface, IToken tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _senhaInterface = senhaInterface;
            _tokenService = tokenService;
        }

        public async Task<UsuarioLoginRespostaDto> Login(UsuarioLoginDto usuarioLogin)
        {

            var usuario = await _usuarioRepository.ObterPorEmail(usuarioLogin.Email.ToLower());
            if (usuario == null)
                throw new NaoEncontradoException("Email inválido!");

            if (!_senhaInterface.VerificaSenhaHash(usuarioLogin.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                throw new ValidacaoException("Senha inválida!");

            var token = _tokenService.CriarToken(usuario);

            var usuarioDto = new UsuarioLeituraDto(usuario.Id, usuario.Nome, usuario.Email);
            return new UsuarioLoginRespostaDto(usuarioDto, token);
        }
    }
}