using Microsoft.AspNetCore.Mvc;
using StatusTasks.WebApi.Dtos;
using StatusTasks.WebApi.Services;

namespace StatusTasks.WebApi.Controllers;

/// <summary>
/// Exposes REST endpoints for querying To-Do items loaded from the JSON data source.
/// </summary>
[ApiController]
[Route("api")]
public sealed class TodosController : ControllerBase
{
    private readonly ITodoService _todoService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodosController"/> class.
    /// </summary>
    /// <param name="todoService">The service that encapsulates To-Do business logic.</param>
    public TodosController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    /// <summary>
    /// Gets all To-Do items.
    /// </summary>
    /// <returns>A 200 response containing all To-Do records.</returns>
    /// <response code="200">Returns all To-Do items.</response>
    [HttpGet("todos")]
    [ProducesResponseType(typeof(IReadOnlyCollection<TodoItemDto>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyCollection<TodoItemDto>> GetTodos()
    {
        return Ok(_todoService.GetAllTodos());
    }

    /// <summary>
    /// Gets all To-Do items for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier. Must be greater than zero.</param>
    /// <returns>A 200 response with the user's To-Do items, 400 for invalid input, or 404 if user does not exist.</returns>
    /// <response code="200">Returns all To-Do items for the requested user.</response>
    /// <response code="400">Returned when <paramref name="userId"/> is less than or equal to zero.</response>
    /// <response code="404">Returned when no To-Do records exist for the requested user.</response>
    [HttpGet("users/{userId:int}/todos")]
    [ProducesResponseType(typeof(IReadOnlyCollection<TodoItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponseDto), StatusCodes.Status404NotFound)]
    public ActionResult<IReadOnlyCollection<TodoItemDto>> GetTodosByUserId(int userId)
    {
        if (userId <= 0)
        {
            return BadRequest(CreateErrorResponse(StatusCodes.Status400BadRequest, "The userId route parameter must be greater than zero."));
        }

        var result = _todoService.GetTodosByUserId(userId);
        if (!result.UserExists)
        {
            return NotFound(CreateErrorResponse(StatusCodes.Status404NotFound, $"No To-Do items were found for userId '{userId}'."));
        }

        return Ok(result.Todos);
    }

    /// <summary>
    /// Gets completed To-Do items for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier. Must be greater than zero.</param>
    /// <returns>A 200 response with completed To-Do items, 400 for invalid input, or 404 if user does not exist.</returns>
    /// <response code="200">Returns completed To-Do items for the requested user.</response>
    /// <response code="400">Returned when <paramref name="userId"/> is less than or equal to zero.</response>
    /// <response code="404">Returned when no To-Do records exist for the requested user.</response>
    [HttpGet("users/{userId:int}/todos/completed")]
    [ProducesResponseType(typeof(IReadOnlyCollection<TodoItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponseDto), StatusCodes.Status404NotFound)]
    public ActionResult<IReadOnlyCollection<TodoItemDto>> GetCompletedTodosByUserId(int userId)
    {
        if (userId <= 0)
        {
            return BadRequest(CreateErrorResponse(StatusCodes.Status400BadRequest, "The userId route parameter must be greater than zero."));
        }

        var result = _todoService.GetCompletedTodosByUserId(userId);
        if (!result.UserExists)
        {
            return NotFound(CreateErrorResponse(StatusCodes.Status404NotFound, $"No To-Do items were found for userId '{userId}'."));
        }

        return Ok(result.Todos);
    }

    /// <summary>
    /// Gets a specific To-Do item by identifier.
    /// </summary>
    /// <param name="id">The To-Do identifier. Must be greater than zero.</param>
    /// <returns>A 200 response with the requested To-Do item, 400 for invalid input, or 404 when not found.</returns>
    /// <response code="200">Returns the requested To-Do item.</response>
    /// <response code="400">Returned when <paramref name="id"/> is less than or equal to zero.</response>
    /// <response code="404">Returned when the requested To-Do item does not exist.</response>
    [HttpGet("todos/{id:int}")]
    [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponseDto), StatusCodes.Status404NotFound)]
    public ActionResult<TodoItemDto> GetTodoById(int id)
    {
        if (id <= 0)
        {
            return BadRequest(CreateErrorResponse(StatusCodes.Status400BadRequest, "The id route parameter must be greater than zero."));
        }

        var todo = _todoService.GetTodoById(id);
        if (todo is null)
        {
            return NotFound(CreateErrorResponse(StatusCodes.Status404NotFound, $"No To-Do item was found with id '{id}'."));
        }

        return Ok(todo);
    }

    private ApiErrorResponseDto CreateErrorResponse(int statusCode, string message)
    {
        return new ApiErrorResponseDto
        {
            StatusCode = statusCode,
            Message = message,
            TraceId = HttpContext.TraceIdentifier,
            TimestampUtc = DateTimeOffset.UtcNow
        };
    }
}
