using ErrorOr;

namespace VsaTemplate.Domain.Todos;

public static class TodoErrors
{
    public static readonly Error TitleRequired = Error.Validation(
        code: "Todo.TitleRequired",
        description: "Todo title is required.");

    public static Error NotFound(Guid id) => Error.NotFound(
        code: "Todo.NotFound",
        description: $"Todo with ID '{id}' was not found.");
}
