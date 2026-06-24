namespace StatusTasks.WebApi.Dtos;

/// <summary>
/// Defines the response contract for a To-Do item returned by API endpoints.
/// </summary>
public sealed class TodoItemDto
{
    /// <summary>
    /// Gets or sets the user identifier that owns the To-Do item.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the To-Do item.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title or short description of the To-Do item.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the To-Do item is completed.
    /// </summary>
    public bool Completed { get; set; }
}
