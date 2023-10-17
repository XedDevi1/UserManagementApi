using FluentValidation;
using Newtonsoft.Json;
using System.Net;
using UserManagementAPI.Exceptions;

namespace UserManagementAPI.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            if (exception is UserNotFoundException)
            {
                code = HttpStatusCode.NotFound;
            }
            else if (exception is EmailAlreadyExistsException)
            {
                code = HttpStatusCode.Conflict;
            }
            else if (exception is BadRequestException)
            {
                code = HttpStatusCode.BadRequest;
            }

            var result = JsonConvert.SerializeObject(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
