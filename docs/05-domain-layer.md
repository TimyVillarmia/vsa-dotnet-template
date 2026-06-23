# 05 - Domain Layer

The Domain layer contains business concepts and business rules.

It lives in:

```txt
VsaTemplate.Domain
```

## Responsibilities

Domain contains:

* entities
* value objects
* domain errors
* domain behavior
* business rules

## Example Structure

```txt
VsaTemplate.Domain/
├── Common/
│   └── Entity.cs
└── Todos/
    ├── Todo.cs
    └── TodoErrors.cs
```

## Entity Base Class

```csharp
public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
}
```

## Todo Entity

```csharp
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

    public void Complete()
    {
        IsCompleted = true;
    }
}
```

## Domain Errors

```csharp
public static class TodoErrors
{
    public static readonly Error TitleRequired = Error.Validation(
        code: "Todo.TitleRequired",
        description: "Todo title is required.");

    public static Error NotFound(Guid id) => Error.NotFound(
        code: "Todo.NotFound",
        description: $"Todo with ID '{id}' was not found.");
}
```

## Domain Rules

Business rules should live in Domain when possible.

Good:

```csharp
Todo.Create(request.Title);
```

Bad:

```csharp
if (string.IsNullOrWhiteSpace(request.Title))
{
    return Results.BadRequest();
}
```

inside every endpoint.

## What Domain Should Not Use

Domain should not use:

* ASP.NET Core
* EF Core
* MediatR
* Serilog
* PostgreSQL
* HTTP status codes
* API request models

Domain should be pure.

