using ErrorOr;
using Project.Domain.Common;

namespace Project.Domain.Todos;

public sealed class Todo : Entity
{
    private Todo()
    {
    }

    private Todo(string title)
    {
        Title = title;
        IsCompleted = false;
    }

    public string Title { get; private set; } = string.Empty;

    public bool IsCompleted { get; private set; }

    public static ErrorOr<Todo> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return TodoErrors.TitleRequired;
        }

        return new Todo(title);
    }

    public void Update(string title, bool isCompleted)
    {
        Title = title;
        IsCompleted = isCompleted;
    }

    public void Complete()
    {
        IsCompleted = true;
    }
}