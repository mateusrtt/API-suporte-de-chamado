using Api.Attributes;
using Api.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace Api.Filters
{
    public class FiltroRespostaSucesso : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult resultadoObjeto && resultadoObjeto.Value != null)
            {
                var metodoAction = (context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)?.MethodInfo;

                var atributoMensagem = metodoAction?.GetCustomAttribute<MensagemSucessoAtributo>();

                var mensagem = atributoMensagem?.Mensagem ?? "Operação realizada com sucesso.";

                var respostaEnvelopada = new Response<object>
                {
                    Dados = resultadoObjeto.Value, 
                    Mensagem = mensagem,           
                    Status = true                  
                };
                context.Result = new OkObjectResult(respostaEnvelopada);
            }
            await next();
        }
    }
}