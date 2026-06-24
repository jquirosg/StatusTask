using StatusTasks.WebApi.Models;

namespace StatusTasks.WebApi.Repositories;

/// <summary>
/// Provides data-access operations for To-Do items loaded from a JSON file.
/// </summary>
public interface ITodoRepository
{
    /// <summary>
    /// Asynchronously loads and indexes the To-Do data source in memory.
    /// </summary>
    /// <param name="cancellationToken">A token that can cancel the asynchronous load.</param>
    /// <returns>A task that completes when the repository state is fully initialized.</returns>
    Task InitializeAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all To-Do items from the in-memory cache.
    /// </summary>
    /// <returns>A read-only collection of all available To-Do items.</returns>
    IReadOnlyCollection<TodoItem> GetAll();

    /// <summary>
    /// Determines whether at least one To-Do item exists for the specified user.
    /// </summary>
    /// <param name="userId">The user identifier to check.</param>
    /// <returns><see langword="true"/> when the user exists; otherwise, <see langword="false"/>.</returns>
    bool UserExists(int userId);

    /// <summary>
    /// Gets all To-Do items for a specific user from indexed in-memory data.
    /// </summary>
    /// <param name="userId">The user identifier to query.</param>
    /// <returns>A read-only collection of To-Do items for the given user.</returns>
    IReadOnlyCollection<TodoItem> GetByUserId(int userId);

    /// <summary>
    /// Gets completed To-Do items for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier to query.</param>
    /// <returns>A read-only collection containing only completed To-Do items for the user.</returns>
    IReadOnlyCollection<TodoItem> GetCompletedByUserId(int userId);

    /// <summary>
    /// Gets a specific To-Do item by its unique identifier.
    /// </summary>
    /// <param name="id">The unique To-Do item identifier.</param>
    /// <returns>The matched To-Do item when found; otherwise <see langword="null"/>.</returns>
    TodoItem? GetById(int id);
}
