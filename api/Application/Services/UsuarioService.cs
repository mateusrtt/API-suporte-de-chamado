using Application.Dtos;
using Application.Interface;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    // Serviços responsável dos usuários
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

        /// <summary>
        /// Registra um novo usuário no sistema, verificando email duplicado,
        /// validando senha e atribuindo o cargo correto.
        /// </summary>
        /// <param name="usuarioRegistro">Dados enviados para criação do usuário.</param>
        /// <param name="cargo">Cargo padrão é 'Usuario', exceto quando um admin cria o usuário.</param>
        /// <returns>Um DTO contendo os dados do usuário recém-criado.</returns>
        /// <exception cref="ConflitoException">Lançado se o email já estiver cadastrado.</exception>
        /// <exception cref="ValidacaoException">Lançado para senhas inválidas ou confirmação incorreta.</exception>
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

            return new UsuarioLeituraDto(usuario.Id, usuario.Nome, usuario.Email, usuario.Cargo);
        }

        /// <summary>
        /// Atualiza os dados do usuário logado, incluindo nome, email e senha.
        /// </summary>
        /// <param name="id">ID do usuário que será atualizado.</param>
        /// <param name="dto">Dados enviados para atualização.</param>
        /// <param name="usuarioLogadoId">ID do usuário autenticado.</param>
        /// <returns>DTO contendo os dados atualizados do usuário.</returns>
        /// <exception cref="NaoAutorizadoException">Lançado quando o usuário tenta atualizar outra conta.</exception>
        /// <exception cref="NaoEncontradoException">Lançado se o usuário não existir.</exception>
        /// <exception cref="ValidacaoException">Lançado em erros de validação de senha.</exception>
        /// <exception cref="ConflitoException">Lançado se o email estiver em uso por outro usuário.</exception>
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

            return new UsuarioLeituraDto(usuario.Id, usuario.Nome, usuario.Email, usuario.Cargo);
        }

        /// <summary>
        /// Remove a conta do usuário logado do sistema.
        /// </summary>
        /// <param name="id">ID do usuário a ser deletado.</param>
        /// <param name="usuarioLogadoId">ID do usuário autenticado.</param>
        /// <returns>True se a exclusão for bem-sucedida.</returns>
        /// <exception cref="NaoAutorizadoException">Lançado quando o usuário tenta excluir outra conta.</exception>
        /// <exception cref="NaoEncontradoException">Lançado quando o usuário não existe.</exception>
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