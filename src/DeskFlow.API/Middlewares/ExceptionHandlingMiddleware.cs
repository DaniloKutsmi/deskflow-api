using System.Net;
using DeskFlow.API.Models;
using DeskFlow.API.Models.Exceptions;

namespace DeskFlow.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await TratamentoExcecao(context, ex);
            }
        }

        private Task TratamentoExcecao(HttpContext context, Exception ex)
        {
            HttpStatusCode status;
            string message;

            if (ex is RegraNegocioException)
            {
                status = HttpStatusCode.BadRequest;
                message = ex.Message;
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
                message = "Ocorreu um erro inesperado. Entre em contato com o suporte.";
            }

            var response = new ErrorDTO(message);

            context.Response.StatusCode = (int)status;

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}