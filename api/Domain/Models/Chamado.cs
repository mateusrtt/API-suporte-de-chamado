using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Models
{
    public class Chamado
    {
        public int Id { get; private set; }
        public string Descricao { get; private set; } = string.Empty;
        public string? Resposta { get; private set; }
        public PrioridadeEnum Prioridade { get; private set; }
        public StatusChamadoEnum Status { get; private set; } = StatusChamadoEnum.Pendente;
        public int UsuarioId { get; private set; }
        public Usuario? Usuario { get; private set; } 
        public DateTime ChamadoDataCriacao { get; private set; } = DateTime.Now;

        protected Chamado() { }

        public Chamado(string descricao, int usuarioId)
        {
            DefinirDescricao(descricao);
            DefinirUsuario(usuarioId);
            Prioridade = PrioridadeEnum.Analisando;
            Status = StatusChamadoEnum.Pendente;
            ChamadoDataCriacao = DateTime.Now;
        }

        public void DefinirDescricao(string descricao)
        {
            if (descricao.Length < 5 || descricao.Length > 500)
                throw new ValidacaoException(
                    "A descrição do chamado deve ter entre 5 e 500 caracteres e não pode ser vazia."
                );

            Descricao = descricao.Trim();
        }

        public void DefinirUsuario(int usuarioId)
        {
            if (usuarioId <= 0)
                throw new ValidacaoException("Usuário inválido para o chamado.");

            UsuarioId = usuarioId;
        }

        public void DefinirPrioridade(PrioridadeEnum prioridade)
        {
            if (!Enum.IsDefined(typeof(PrioridadeEnum), prioridade))
                prioridade = PrioridadeEnum.Analisando;

            Prioridade = prioridade;
        }

        public void AtualizarPrioridade(PrioridadeEnum prioridadeSugestao)
        {
            if (prioridadeSugestao != PrioridadeEnum.Analisando)
                DefinirPrioridade(prioridadeSugestao);
        }

        public void Responder(string resposta)
        {
            if (resposta.Length < 5 || resposta.Length > 500)
            {
                throw new ValidacaoException(
                    "A resposta do chamado deve ter entre 5 e 500 caracteres e não pode ser vazia."
                );
            }

            if (Status == StatusChamadoEnum.Respondido)
                throw new ValidacaoException("Este chamado já foi respondido.");

            Resposta = resposta;
            Status = StatusChamadoEnum.Respondido;
        }
    }
}