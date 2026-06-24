using System.Text.Json;
using StatusTasks.WebApi.Models;

namespace StatusTasks.WebApi.Repositories;

/// <summary>
/// Implements an in-memory To-Do repository backed by the JSON file in Resources.
/// </summary>
public sealed class JsonTodoRepository : ITodoRepository
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<JsonTodoRepository> _logger;
    private readonly SemaphoreSlim _initializationLock = new(1, 1);

    private List<TodoItem> _todos = new();
    private Dictionary<int, TodoItem> _todosById = new();
    private Dictionary<int, List<TodoItem>> _todosByUserId = new();
    private bool _isInitialized;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonTodoRepository"/> class.
    /// </summary>
    /// <param name="environment">Provides access to the content root for resolving data file paths.</param>
    /// <param name="logger">The logger used for repository diagnostics.</param>
    public JsonTodoRepository(IWebHostEnvironment environment, ILogger<JsonTodoRepository> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (_isInitialized)
        {
            return;
        }

        await _initializationLock.WaitAsync(cancellationToken);
        try
        {
            if (_isInitialized)
            {
                return;
            }

            var filePath = Path.Combine(_environment.ContentRootPath, "Resources", "todos.json");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The To-Do data source was not found.", filePath);
            }

            await using var stream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 4096,
                useAsync: true);

            var loadedTodos = await JsonSerializer.DeserializeAsync<List<TodoItem>>(
                stream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                cancellationToken) ?? new List<TodoItem>();

            _todos = loadedTodos;
            _todosById = _todos.ToDictionary(todo => todo.Id);
            _todosByUserId = _todos
                .GroupBy(todo => todo.UserId)
                .ToDictionary(group => group.Key, group => group.ToList());

            _isInitialized = true;
            _logger.LogInformation("Loaded {TodoCount} To-Do items from {FilePath}.", _todos.Count, filePath);
        }
        finally
        {
            _initializationLock.Release();
        }
    }

    /// <inheritdoc />
    public IReadOnlyCollection<TodoItem> GetAll()
    {
        EnsureInitialized();
        return _todos;
    }

    /// <inheritdoc />
    public bool UserExists(int userId)
    {
        EnsureInitialized();
        return _todosByUserId.ContainsKey(userId);
    }

    /// <inheritdoc />
    public IReadOnlyCollection<TodoItem> GetByUserId(int userId)
    {
        EnsureInitialized();
        return _todosByUserId.TryGetValue(userId, out var todos) ? todos : Array.Empty<TodoItem>();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<TodoItem> GetCompletedByUserId(int userId)
    {
        EnsureInitialized();
        if (!_todosByUserId.TryGetValue(userId, out var todos))
        {
            return Array.Empty<TodoItem>();
        }

        return todos.Where(todo => todo.Completed).ToArray();
    }

    /// <inheritdoc />
    public TodoItem? GetById(int id)
    {
        EnsureInitialized();
        return _todosById.TryGetValue(id, out var todo) ? todo : null;
    }

    private void EnsureInitialized()
    {
        if (_isInitialized)
        {
            return;
        }

        throw new InvalidOperationException("The To-Do repository has not been initialized. Call InitializeAsync first.");
    }
}
