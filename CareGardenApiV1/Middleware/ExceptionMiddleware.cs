using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Model;
using System.Text.Json;

namespace CareGardenApiV1.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILoggerHandler _loggerHandler;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(ILoggerHandler loggerHandler, IHostEnvironment env)
        {
            _loggerHandler = loggerHandler;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            response.HasError = true;
            response.Message = "StatusCode : " + context.Response.StatusCode + ". " + ex.Message;
            response.Data = ex.StackTrace;

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
