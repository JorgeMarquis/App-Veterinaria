using System.Text.Json;
using Veterinaria.Shared.Exceptions;

namespace Veterinaria.API.Middleware;

/// <summary>
/// Middleware para manejar excepciones globales en la API
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción no manejada");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new { message = exception.Message };

        return exception switch
        {
            NotFoundException => SetResponse(context, StatusCodes.Status404NotFound, response),

            ValidationException validationEx => SetResponse(context, StatusCodes.Status400BadRequest, 
                new { message = exception.Message, errors = validationEx.Errors }),

            ArgumentNullException or ArgumentException => SetResponse(context, StatusCodes.Status400BadRequest,
                new { message = "Parámetros requeridos no proporcionados o inválidos." }),

            UnauthorizedAccessException => SetResponse(context, StatusCodes.Status401Unauthorized,
                new { message = "No autorizado para acceder a este recurso." }),

            KeyNotFoundException => SetResponse(context, StatusCodes.Status404NotFound,
                new { message = "Recurso no encontrado." }),

            InvalidOperationException => SetResponse(context, StatusCodes.Status400BadRequest,
                new { message = "Operación inválida. " + exception.Message }),

            _ => SetResponse(context, StatusCodes.Status500InternalServerError, 
                new { message = "Ocurrió un error interno en el servidor. Por favor intente más tarde." })
        };
    }

    private static Task SetResponse(HttpContext context, int statusCode, object response)
    {
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsJsonAsync(response);
    }
}
