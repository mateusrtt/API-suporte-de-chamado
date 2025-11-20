using Application.Interface;
using Domain.Interfaces;
using Application.Dtos;
using Domain.Exceptions;

namespace Application.Services
{
    // Serviço responsável pela autenticação
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

        /// <summary>
        /// Realiza o processo de login verificando email e senha,
        /// e retorna um token juntamente com os dados do usuário.
        /// </summary>
        /// <param name="usuarioLogin">DTO contendo email e senha enviados pelo usuário.</param>
        /// <returns>Um objeto contendo os dados do usuário e o token JWT.</returns>
        /// <exception cref="NaoEncontradoException">Lançado quando o email não existe no sistema.</exception>
        /// <exception cref="ValidacaoException">Lançado quando a senha é inválida.</exception>
        public async Task<UsuarioLoginRespostaDto> Login(UsuarioLoginDto usuarioLogin)
        {

            var usuario = await _usuarioRepository.ObterPorEmail(usuarioLogin.Email.ToLower());
            if (usuario == null)
                throw new NaoEncontradoException("Email inválido!");

            if (!_senhaInterface.VerificaSenhaHash(usuarioLogin.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                throw new ValidacaoException("Senha inválida!");

            var token = _tokenService.CriarToken(usuario);

            var usuarioDto = new UsuarioLeituraDto(usuario.Id, usuario.Nome, usuario.Email, usuario.Cargo);
            return new UsuarioLoginRespostaDto(usuarioDto, token);
        }
    }
}