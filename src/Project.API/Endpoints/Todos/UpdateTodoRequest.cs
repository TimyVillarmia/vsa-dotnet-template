namespace Project.API.Endpoints.Todos;

public sealed record UpdateTodoRequest(string Title, bool IsCompleted);