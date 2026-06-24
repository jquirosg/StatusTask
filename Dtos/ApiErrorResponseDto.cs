namespace StatusTasks.WebApi.Dtos;

/// <summary>
/// Defines the standard error payload returned by the API for handled failures.
/// </summary>
public sealed class ApiErrorResponseDto
{
    /// <summary>
    /// Gets or sets the HTTP status code associated with the error.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Gets or sets a user-friendly message that describes the error.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a trace identifier used for diagnostics.
    /// </summary>
    public string TraceId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTC timestamp for when the error occurred.
    /// </summary>
    public DateTimeOffset TimestampUtc { get; set; }
}
