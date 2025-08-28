using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api_sistema_de_chamado.Filters
{
    public class FiltroRespostaSucesso : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult objectResult && objectResult.Value != null)
            {
                var mensagem = context.HttpContext.Items.ContainsKey("MensagemSucesso")
                    ? context.HttpContext.Items["MensagemSucesso"]?.ToString()
                    : "Operação realizada com sucesso.";

                var envelopado = new
                {
                    Dados = objectResult.Value,
                    Mensagem = mensagem,
                    Status = true
                };

                context.Result = new OkObjectResult(envelopado);
            }
            await next();
        }
    }
}