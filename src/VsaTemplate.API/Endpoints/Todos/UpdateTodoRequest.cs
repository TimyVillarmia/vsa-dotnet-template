namespace VsaTemplate.API.Endpoints.Todos;

public sealed record UpdateTodoRequest(string Title, bool IsCompleted);