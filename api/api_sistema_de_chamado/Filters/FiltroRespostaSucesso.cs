using Api.Attributes;
using Api.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace Api.Filters
{
    /// <summary>
    /// Filtro que padroniza respostas de sucesso da API.
    /// Intercepta ObjectResult não nulo e o envelopa em um Response&lt;object&gt; com Dados, Mensagem e Status.
    /// A Mensagem é obtida do atributo MensagemSucessoAtributo ou usa "Operação realizada com sucesso." como padrão.
    /// </summary>
    public class FiltroRespostaSucesso : IAsyncResultFilter
    {
        /// <summary>
        /// Executa o filtro, interceptando o resultado da ação e padronizando a resposta de sucesso.
        /// </summary>
        /// <param name="context">Contexto do resultado (ResultExecutingContext) contendo informações da ação e do resultado.</param>
        /// <param name="next">Delegate para continuar a execução do pipeline (ResultExecutionDelegate).</param>
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