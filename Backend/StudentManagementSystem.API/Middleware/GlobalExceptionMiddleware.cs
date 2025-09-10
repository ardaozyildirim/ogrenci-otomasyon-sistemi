using System.Net;
using System.Text.Json;

namespace StudentManagementSystem.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        object response;
        int statusCode;

        switch (exception)
        {
            case ArgumentNullException:
                statusCode = (int)HttpStatusCode.BadRequest;
                response = new
                {
                    error = new
                    {
                        message = "Required parameter is missing",
                        details = exception.Message,
                        timestamp = DateTime.UtcNow
                    }
                };
                break;
            case ArgumentException:
                statusCode = (int)HttpStatusCode.BadRequest;
                response = new
                {
                    error = new
                    {
                        message = "Invalid request parameters",
                        details = exception.Message,
                        timestamp = DateTime.UtcNow
                    }
                };
                break;
            case UnauthorizedAccessException:
                statusCode = (int)HttpStatusCode.Unauthorized;
                response = new
                {
                    error = new
                    {
                        message = "Unauthorized access",
                        details = exception.Message,
                        timestamp = DateTime.UtcNow
                    }
                };
                break;
            case InvalidOperationException:
                statusCode = (int)HttpStatusCode.Conflict;
                response = new
                {
                    error = new
                    {
                        message = "Operation conflict",
                        details = exception.Message,
                        timestamp = DateTime.UtcNow
                    }
                };
                break;
            default:
                statusCode = (int)HttpStatusCode.InternalServerError;
                response = new
                {
                    error = new
                    {
                        message = "An internal server error occurred",
                        timestamp = DateTime.UtcNow
                    }
                };
                break;
        }

        context.Response.StatusCode = statusCode;

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}
