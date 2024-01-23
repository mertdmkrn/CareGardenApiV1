using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using System.Globalization;
using System.Text.Json;

namespace CareGardenApiV1.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILoggerHandler _loggerHandler;

        public ExceptionMiddleware(ILoggerHandler loggerHandler)
        {
            _loggerHandler = loggerHandler;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                Resource.Resource.Culture = new CultureInfo(context.Request.Headers["Language"].ToString().IsNull("en"));
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
            ResponseModel<bool> response = new ResponseModel<bool>();
            response.Message = $"Exception : {ex.Message} | {context.Request.Path}";
            response.HasError = true;

            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
