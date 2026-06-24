namespace VsaTemplate.Application.Features.Todos.GetTodoById;

public sealed record GetTodoByIdResponse(
    Guid Id,
    string Title,
    bool IsCompleted);
