using System.Net;

namespace Library.Api.Host.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Exception caught. Path: {Path}, Method: {Method}, Message: {Message}",
                context.Request.Path,
                context.Request.Method,
                ex.Message);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            ArgumentException or ArgumentNullException or FormatException =>
                ((int)HttpStatusCode.BadRequest, "Некорректные данные. Проверьте формат запроса."),
            KeyNotFoundException =>
                ((int)HttpStatusCode.NotFound, "Запрашиваемый ресурс не найден."),
            InvalidOperationException =>
                ((int)HttpStatusCode.Conflict, "Невозможно выполнить операцию. Попробуйте позже."),
            _ => ((int)HttpStatusCode.InternalServerError, "Произошла внутренняя ошибка. Попробуйте позже.")
        };

        context.Response.StatusCode = statusCode;

        var response = new
        {
            message,
            statusCode,
            timestamp = DateTime.UtcNow
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}
