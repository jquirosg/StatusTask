using System.Text.Json.Serialization;

namespace StatusTasks.WebApi.Models;

/// <summary>
/// Represents the persisted To-Do item schema loaded from the JSON data source.
/// </summary>
public sealed class TodoItem
{
    /// <summary>
    /// Gets or sets the user identifier that owns the To-Do item.
    /// </summary>
    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the To-Do item.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title or short description of the To-Do item.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the To-Do item is completed.
    /// </summary>
    [JsonPropertyName("completed")]
    public bool Completed { get; set; }
}
