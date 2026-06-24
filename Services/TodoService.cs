using StatusTasks.WebApi.Dtos;
using StatusTasks.WebApi.Models;
using StatusTasks.WebApi.Repositories;

namespace StatusTasks.WebApi.Services;

/// <summary>
/// Implements business logic for To-Do query operations.
/// </summary>
public sealed class TodoService : ITodoService
{
    private readonly ITodoRepository _todoRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoService"/> class.
    /// </summary>
    /// <param name="todoRepository">The repository used to access To-Do data.</param>
    public TodoService(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    /// <inheritdoc />
    public IReadOnlyCollection<TodoItemDto> GetAllTodos()
    {
        return _todoRepository.GetAll().Select(MapToDto).ToArray();
    }

    /// <inheritdoc />
    public (bool UserExists, IReadOnlyCollection<TodoItemDto> Todos) GetTodosByUserId(int userId)
    {
        if (!_todoRepository.UserExists(userId))
        {
            return (false, Array.Empty<TodoItemDto>());
        }

        var todos = _todoRepository.GetByUserId(userId).Select(MapToDto).ToArray();
        return (true, todos);
    }

    /// <inheritdoc />
    public (bool UserExists, IReadOnlyCollection<TodoItemDto> Todos) GetCompletedTodosByUserId(int userId)
    {
        if (!_todoRepository.UserExists(userId))
        {
            return (false, Array.Empty<TodoItemDto>());
        }

        var todos = _todoRepository.GetCompletedByUserId(userId).Select(MapToDto).ToArray();
        return (true, todos);
    }

    /// <inheritdoc />
    public TodoItemDto? GetTodoById(int id)
    {
        var todo = _todoRepository.GetById(id);
        return todo is null ? null : MapToDto(todo);
    }

    private static TodoItemDto MapToDto(TodoItem todo)
    {
        return new TodoItemDto
        {
            UserId = todo.UserId,
            Id = todo.Id,
            Title = todo.Title,
            Completed = todo.Completed
        };
    }
}
