namespace StudentManagementSystem.API.Models;

/// <summary>
/// Standard error response model
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Error type identifier
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Error title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Error status code
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Detailed error message
    /// </summary>
    public string Detail { get; set; } = string.Empty;

    /// <summary>
    /// Error instance (request path)
    /// </summary>
    public string Instance { get; set; } = string.Empty;

    /// <summary>
    /// List of validation errors
    /// </summary>
    public Dictionary<string, string[]>? Errors { get; set; }

    /// <summary>
    /// Error timestamp
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Creates a validation error response
    /// </summary>
    public static ErrorResponse ValidationError(string title, Dictionary<string, string[]> errors, string instance = "")
    {
        return new ErrorResponse
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = title,
            Status = 400,
            Detail = "One or more validation errors occurred.",
            Instance = instance,
            Errors = errors
        };
    }

    /// <summary>
    /// Creates a not found error response
    /// </summary>
    public static ErrorResponse NotFound(string title, string detail, string instance = "")
    {
        return new ErrorResponse
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = title,
            Status = 404,
            Detail = detail,
            Instance = instance
        };
    }

    /// <summary>
    /// Creates an unauthorized error response
    /// </summary>
    public static ErrorResponse Unauthorized(string title, string detail, string instance = "")
    {
        return new ErrorResponse
        {
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
            Title = title,
            Status = 401,
            Detail = detail,
            Instance = instance
        };
    }

    /// <summary>
    /// Creates a forbidden error response
    /// </summary>
    public static ErrorResponse Forbidden(string title, string detail, string instance = "")
    {
        return new ErrorResponse
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            Title = title,
            Status = 403,
            Detail = detail,
            Instance = instance
        };
    }

    /// <summary>
    /// Creates an internal server error response
    /// </summary>
    public static ErrorResponse InternalServerError(string title, string detail, string instance = "")
    {
        return new ErrorResponse
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = title,
            Status = 500,
            Detail = detail,
            Instance = instance
        };
    }
}
