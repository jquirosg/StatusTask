using StatusTasks.WebApi.Models;
using StatusTasks.WebApi.Repositories;
using StatusTasks.WebApi.Services;

namespace StatusTasks.WebApi.Tests;

public sealed class TodoServiceTests
{
    [Fact]
    public void GetAllTodos_ReturnsMappedDtos()
    {
        // Arrange
        var repository = new FakeTodoRepository
        {
            AllTodos = new[]
            {
                new TodoItem { UserId = 1, Id = 10, Title = "Task A", Completed = false },
                new TodoItem { UserId = 2, Id = 20, Title = "Task B", Completed = true }
            }
        };
        var service = new TodoService(repository);

        // Act
        var result = service.GetAllTodos();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, item => item.Id == 10 && item.UserId == 1 && item.Title == "Task A" && !item.Completed);
        Assert.Contains(result, item => item.Id == 20 && item.UserId == 2 && item.Title == "Task B" && item.Completed);
    }

    [Fact]
    public void GetTodosByUserId_WhenUserDoesNotExist_ReturnsEmptyAndFalse()
    {
        // Arrange
        var repository = new FakeTodoRepository { UserExistsResult = false };
        var service = new TodoService(repository);

        // Act
        var result = service.GetTodosByUserId(999);

        // Assert
        Assert.False(result.UserExists);
        Assert.Empty(result.Todos);
    }

    [Fact]
    public void GetTodosByUserId_WhenUserExists_ReturnsMappedDtosAndTrue()
    {
        // Arrange
        var repository = new FakeTodoRepository
        {
            UserExistsResult = true,
            TodosByUser = new[]
            {
                new TodoItem { UserId = 5, Id = 1, Title = "Task 1", Completed = false },
                new TodoItem { UserId = 5, Id = 2, Title = "Task 2", Completed = true }
            }
        };
        var service = new TodoService(repository);

        // Act
        var result = service.GetTodosByUserId(5);

        // Assert
        Assert.True(result.UserExists);
        Assert.Equal(2, result.Todos.Count);
        Assert.All(result.Todos, item => Assert.Equal(5, item.UserId));
    }

    [Fact]
    public void GetCompletedTodosByUserId_WhenUserExists_ReturnsOnlyCompleted()
    {
        // Arrange
        var repository = new FakeTodoRepository
        {
            UserExistsResult = true,
            CompletedTodosByUser = new[]
            {
                new TodoItem { UserId = 4, Id = 15, Title = "Done 1", Completed = true },
                new TodoItem { UserId = 4, Id = 16, Title = "Done 2", Completed = true }
            }
        };
        var service = new TodoService(repository);

        // Act
        var result = service.GetCompletedTodosByUserId(4);

        // Assert
        Assert.True(result.UserExists);
        Assert.Equal(2, result.Todos.Count);
        Assert.All(result.Todos, item => Assert.True(item.Completed));
    }

    [Fact]
    public void GetTodoById_WhenTodoExists_ReturnsMappedDto()
    {
        // Arrange
        var repository = new FakeTodoRepository
        {
            TodoById = new TodoItem { UserId = 7, Id = 50, Title = "Lookup", Completed = true }
        };
        var service = new TodoService(repository);

        // Act
        var result = service.GetTodoById(50);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(50, result!.Id);
        Assert.Equal(7, result.UserId);
        Assert.True(result.Completed);
    }

    [Fact]
    public void GetTodoById_WhenTodoDoesNotExist_ReturnsNull()
    {
        // Arrange
        var repository = new FakeTodoRepository { TodoById = null };
        var service = new TodoService(repository);

        // Act
        var result = service.GetTodoById(404);

        // Assert
        Assert.Null(result);
    }

    private sealed class FakeTodoRepository : ITodoRepository
    {
        public IReadOnlyCollection<TodoItem> AllTodos { get; set; } = Array.Empty<TodoItem>();
        public bool UserExistsResult { get; set; }
        public IReadOnlyCollection<TodoItem> TodosByUser { get; set; } = Array.Empty<TodoItem>();
        public IReadOnlyCollection<TodoItem> CompletedTodosByUser { get; set; } = Array.Empty<TodoItem>();
        public TodoItem? TodoById { get; set; }

        public Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public IReadOnlyCollection<TodoItem> GetAll()
        {
            return AllTodos;
        }

        public bool UserExists(int userId)
        {
            return UserExistsResult;
        }

        public IReadOnlyCollection<TodoItem> GetByUserId(int userId)
        {
            return TodosByUser;
        }

        public IReadOnlyCollection<TodoItem> GetCompletedByUserId(int userId)
        {
            return CompletedTodosByUser;
        }

        public TodoItem? GetById(int id)
        {
            return TodoById;
        }
    }
}
