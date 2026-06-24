using StatusTasks.WebApi.Dtos;

namespace StatusTasks.WebApi.Services;

/// <summary>
/// Defines business operations for querying To-Do items.
/// </summary>
public interface ITodoService
{
    /// <summary>
    /// Gets all available To-Do items.
    /// </summary>
    /// <returns>A read-only collection of To-Do item DTOs.</returns>
    IReadOnlyCollection<TodoItemDto> GetAllTodos();

    /// <summary>
    /// Gets all To-Do items for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier to query.</param>
    /// <returns>
    /// A tuple where <c>UserExists</c> indicates whether the user has any records,
    /// and <c>Todos</c> contains the user's To-Do items when found.
    /// </returns>
    (bool UserExists, IReadOnlyCollection<TodoItemDto> Todos) GetTodosByUserId(int userId);

    /// <summary>
    /// Gets completed To-Do items for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier to query.</param>
    /// <returns>
    /// A tuple where <c>UserExists</c> indicates whether the user has any records,
    /// and <c>Todos</c> contains only completed To-Do items when found.
    /// </returns>
    (bool UserExists, IReadOnlyCollection<TodoItemDto> Todos) GetCompletedTodosByUserId(int userId);

    /// <summary>
    /// Gets a specific To-Do item by identifier.
    /// </summary>
    /// <param name="id">The unique To-Do item identifier.</param>
    /// <returns>The matched To-Do item DTO when found; otherwise <see langword="null"/>.</returns>
    TodoItemDto? GetTodoById(int id);
}
