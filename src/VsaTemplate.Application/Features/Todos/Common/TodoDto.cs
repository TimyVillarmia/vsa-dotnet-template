namespace VsaTemplate.Application.Features.Todos.Common;

public sealed record TodoDto(
    Guid Id,
    string Title,
    bool IsCompleted);
