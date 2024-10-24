using System.Net;
using Newtonsoft.Json;

namespace ApiArchetype.Exceptions;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = JsonConvert.SerializeObject(new
        {
            message = "Internal Server Error.",
            details = exception.Message
        });

        if (exception is UnauthorizedAccessException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            result = JsonConvert.SerializeObject(new { message = "Unauthorized Access" });
        }

        return context.Response.WriteAsync(result);
    }
}
