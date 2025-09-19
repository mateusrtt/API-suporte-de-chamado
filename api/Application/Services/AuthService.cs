using System.Text.RegularExpressions;
using Application.Interface;
using Domain.Interfaces;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Application.Dtos;

namespace Application.Services
{
    public class AuthService : IAuth
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ISenha _senhaInterface;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAcessoLogRepository _acessoLogRepository;
        

        public AuthService(IUsuarioRepository usuarioRepository, ISenha senhaInterface, IHttpContextAccessor httpContextAccessor, IAcessoLogRepository acessoLogRepository)
        {
            _usuarioRepository = usuarioRepository;
            _senhaInterface = senhaInterface;
            _httpContextAccessor = httpContextAccessor;
            _acessoLogRepository = acessoLogRepository;
        }

        public async Task<UsuarioLeituraDto> Registrar(UsuarioCriacaoDto usuarioRegistro, CargoEnum cargo, bool registrarLog = true)
        {
            var usuarioExistente = await _usuarioRepository.ObterPorEmailAsync(usuarioRegistro.Email.ToLower());
            if (usuarioExistente != null)
                throw new ApplicationException("Email já cadastrado.");

            if (string.IsNullOrWhiteSpace(usuarioRegistro.Senha) || !Regex.IsMatch(usuarioRegistro.Senha, @"^.{5,10}$"))
                throw new ApplicationException("Senha inválida: deve ter entre 5 e 10 caracteres");

            if (usuarioRegistro.Senha != usuarioRegistro.ConfirmarSenha)
                throw new ApplicationException("Senha e confirmação não coincidem");

            _senhaInterface.CriarSenhaHash(usuarioRegistro.Senha, out byte[] senhaHash, out byte[] senhaSalt);

            Usuario usuario = new Usuario(
                usuarioRegistro.Nome,
                usuarioRegistro.Email.ToLower(),
                cargo,
                senhaHash,
                senhaSalt,
                usuarioRegistro.AceitouPoliticaPrivacidade
            );

            await _usuarioRepository.AdicionarAsync(usuario);
            await _usuarioRepository.SalvarAsync();

            return new UsuarioLeituraDto(usuario.Id, usuario.Nome, usuario.Email);
        }

        public async Task<UsuarioLoginRespostaDto> Login(UsuarioLoginDto usuarioLogin)
        {

            var usuario = await _usuarioRepository.ObterPorEmailAsync(usuarioLogin.Email.ToLower());
            if (usuario == null)
                throw new ApplicationException("Email inválido!");

            if (!_senhaInterface.VerificaSenhaHash(usuarioLogin.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                throw new ApplicationException("Senha inválida!");

            var token = _senhaInterface.CriarToken(usuario);

            var usuarioDto = new UsuarioLeituraDto(usuario.Id, usuario.Nome, usuario.Email);
            return new UsuarioLoginRespostaDto(usuarioDto, token);
        }
    }
}