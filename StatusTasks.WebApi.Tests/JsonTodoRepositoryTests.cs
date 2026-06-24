using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using StatusTasks.WebApi.Repositories;

namespace StatusTasks.WebApi.Tests;

public sealed class JsonTodoRepositoryTests
{
    [Fact]
    public void GetAll_BeforeInitialize_ThrowsInvalidOperationException()
    {
        // Arrange
        var root = CreateTempRoot();
        var repository = new JsonTodoRepository(CreateHostEnvironment(root), CreateLogger());

        try
        {
            // Act + Assert
            Assert.Throws<InvalidOperationException>(() => repository.GetAll());
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public async Task InitializeAsync_LoadsTodos_AndEnablesQueries()
    {
        // Arrange
        var root = CreateTempRoot();
        var resourcesPath = Path.Combine(root, "Resources");
        Directory.CreateDirectory(resourcesPath);

        var sampleJson = """
[
  { "userId": 1, "id": 1, "title": "Task A", "completed": false },
  { "userId": 1, "id": 2, "title": "Task B", "completed": true },
  { "userId": 2, "id": 3, "title": "Task C", "completed": true }
]
""";
        await File.WriteAllTextAsync(Path.Combine(resourcesPath, "todos.json"), sampleJson);

        var repository = new JsonTodoRepository(CreateHostEnvironment(root), CreateLogger());

        try
        {
            // Act
            await repository.InitializeAsync();

            // Assert
            var all = repository.GetAll();
            Assert.Equal(3, all.Count);

            Assert.True(repository.UserExists(1));
            Assert.False(repository.UserExists(999));

            var user1Todos = repository.GetByUserId(1);
            Assert.Equal(2, user1Todos.Count);

            var user1Completed = repository.GetCompletedByUserId(1);
            Assert.Single(user1Completed);
            Assert.All(user1Completed, item => Assert.True(item.Completed));

            var byId = repository.GetById(3);
            Assert.NotNull(byId);
            Assert.Equal("Task C", byId!.Title);

            Assert.Null(repository.GetById(404));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private static IWebHostEnvironment CreateHostEnvironment(string contentRootPath)
    {
        return new TestWebHostEnvironment
        {
            ApplicationName = "StatusTasks.WebApi.Tests",
            EnvironmentName = "Development",
            ContentRootPath = contentRootPath,
            ContentRootFileProvider = new NullFileProvider(),
            WebRootPath = contentRootPath,
            WebRootFileProvider = new NullFileProvider()
        };
    }

    private static ILogger<JsonTodoRepository> CreateLogger()
    {
        var loggerFactory = LoggerFactory.Create(builder => { });
        return loggerFactory.CreateLogger<JsonTodoRepository>();
    }

    private static string CreateTempRoot()
    {
        var root = Path.Combine(Path.GetTempPath(), "StatusTasksTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        return root;
    }

    private sealed class TestWebHostEnvironment : IWebHostEnvironment
    {
        public string ApplicationName { get; set; } = string.Empty;

        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();

        public string WebRootPath { get; set; } = string.Empty;

        public string EnvironmentName { get; set; } = string.Empty;

        public string ContentRootPath { get; set; } = string.Empty;

        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
