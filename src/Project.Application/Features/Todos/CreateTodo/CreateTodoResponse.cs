namespace Project.Application.Features.Todos.CreateTodo;

public sealed record CreateTodoResponse(
    Guid Id,
    string Title,
    bool IsCompleted);