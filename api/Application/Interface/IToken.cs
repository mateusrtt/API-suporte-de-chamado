using Domain.Models;

namespace Application.Interface
{
    public interface IToken
    {
        string CriarToken(Usuario usuario);
    }
}
