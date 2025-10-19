using Application.Dtos;
using Application.Interface;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class UsuarioService : IUsuario
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ISenha _senhaInterface;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public UsuarioService(IUsuarioRepository usuarioRepository, ISenha senhaInterface, IHttpContextAccessor httpContextAccessor)
        {
            _usuarioRepository = usuarioRepository;
            _senhaInterface = senhaInterface;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UsuarioLeituraDto> Registrar(UsuarioCriacaoDto usuarioRegistro, CargoEnum cargo = CargoEnum.Usuario)
        {
            if (await _usuarioRepository.EmailJaCadastrado(usuarioRegistro.Email))
                throw new ConflitoException("Email já cadastrado.");

            Usuario.ValidarSenha(usuarioRegistro.Senha);

            if (usuarioRegistro.Senha != usuarioRegistro.ConfirmarSenha)
                throw new ValidacaoException("Senha e confirmação não coincidem");

            _senhaInterface.CriarSenhaHash(usuarioRegistro.Senha, out byte[] senhaHash, out byte[] senhaSalt);

            var usuarioLogado = _httpContextAccessor.HttpContext?.User;
            if (usuarioLogado != null && usuarioLogado.IsInRole("Administrador"))
                cargo = CargoEnum.Administrador;

            Usuario usuario = new Usuario(
                usuarioRegistro.Nome,
                usuarioRegistro.Email.ToLower(),
                cargo,
                senhaHash,
                senhaSalt,
                usuarioRegistro.AceitouPoliticaPrivacidade
            );

            await _usuarioRepository.Adicionar(usuario);
            await _usuarioRepository.Salvar();

            return new UsuarioLeituraDto(usuario.Id, usuario.Nome, usuario.Email);
        }

        public async Task<UsuarioLeituraDto> Atualizar(int id, UsuarioUpdateDto dto, int usuarioLogadoId)
        {
            if (id != usuarioLogadoId)
                throw new NaoAutorizadoException("Você só pode atualizar seus próprios dados.");

            var usuario = await _usuarioRepository.ObterPorId(id);
            if (usuario == null)
                throw new NaoEncontradoException("Usuário não encontrado.");

            if (!string.IsNullOrWhiteSpace(dto.Nome))
                usuario.DefinirNome(dto.Nome);

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                var usuarioExistente = await _usuarioRepository.ObterPorEmail(dto.Email.ToLower());
                if (usuarioExistente != null && usuarioExistente.Id != usuario.Id)
                    throw new ConflitoException("Este email já está em uso por outro usuário.");

                usuario.DefinirEmail(dto.Email);
            }

            if (!string.IsNullOrWhiteSpace(dto.NovaSenha) || !string.IsNullOrWhiteSpace(dto.ConfirmarNovaSenha))
            {
                var senhaAtual = dto.SenhaAtual ?? throw new ValidacaoException("Para alterar a senha é necessário informar a senha atual.");
                var novaSenha = dto.NovaSenha!;
                var confirmarSenha = dto.ConfirmarNovaSenha!;

                if (!_senhaInterface.VerificaSenhaHash(senhaAtual, usuario.SenhaHash, usuario.SenhaSalt))
                    throw new ValidacaoException("Senha atual incorreta.");

                if (novaSenha != confirmarSenha)
                    throw new ValidacaoException("A nova senha e a confirmação não conferem.");

                Usuario.ValidarSenha(novaSenha);

                _senhaInterface.CriarSenhaHash(novaSenha, out byte[] hash, out byte[] salt);

                usuario.DefinirSenha(hash, salt);
            }

            await _usuarioRepository.Atualizar(usuario);
            await _usuarioRepository.Salvar();

            return new UsuarioLeituraDto(usuario.Id, usuario.Nome, usuario.Email);
        }

        public async Task<bool> Deletar(int id, int usuarioLogadoId)
        {
            if (id != usuarioLogadoId)
                throw new NaoAutorizadoException("Você só pode deletar sua própria conta.");

            var usuario = await _usuarioRepository.ObterPorId(id);
            if (usuario == null)
                throw new NaoEncontradoException("Usuário não encontrado.");

            _usuarioRepository.Deletar(usuario);
            await _usuarioRepository.Salvar();

            return true;
        }
    }
}