# 04 - Application Layer

The Application layer contains use cases.

It lives in:

```txt
VsaTemplate.Application
```

## Responsibilities

Application contains:

* commands
* queries
* handlers
* validators
* responses
* DTOs
* application abstractions
* MediatR pipeline behaviors

## Feature-Based Structure

Example:

```txt
VsaTemplate.Application/
└── Features/
    └── Todos/
        ├── CreateTodo/
        │   ├── CreateTodoCommand.cs
        │   ├── CreateTodoHandler.cs
        │   ├── CreateTodoValidator.cs
        │   └── CreateTodoResponse.cs
        │
        ├── GetTodoById/
        │   ├── GetTodoByIdQuery.cs
        │   ├── GetTodoByIdHandler.cs
        │   └── GetTodoByIdResponse.cs
        │
        └── Common/
            └── TodoDto.cs
```

## Commands and Queries

Command means changing state.

Query means reading state.

Current Todo use cases:

```txt
CreateTodoCommand
GetTodoByIdQuery
```

Future features can add more commands and queries beside these folders.

## Command Example

```csharp
public sealed record CreateTodoCommand(string Title)
    : IRequest<ErrorOr<CreateTodoResponse>>;
```

## Query Example

```csharp
public sealed record GetTodoByIdQuery(Guid Id)
    : IRequest<ErrorOr<GetTodoByIdResponse>>;
```

## Handler Example

```csharp
public sealed class CreateTodoHandler
    : IRequestHandler<CreateTodoCommand, ErrorOr<CreateTodoResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateTodoHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<CreateTodoResponse>> Handle(
        CreateTodoCommand request,
        CancellationToken cancellationToken)
    {
        var todoResult = Todo.Create(request.Title);

        if (todoResult.IsError)
        {
            return todoResult.Errors;
        }

        var todo = todoResult.Value;

        _dbContext.Todos.Add(todo);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateTodoResponse(
            todo.Id,
            todo.Title,
            todo.IsCompleted);
    }
}
```

## Validators

Validators live beside the command/query.

```csharp
public sealed class CreateTodoValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoValidator()
    {
        RuleFor(command => command.Title)
            .NotEmpty()
            .MaximumLength(200);
    }
}
```

## Pipeline Behaviors

Pipeline behaviors run around handlers.

Current behaviors:

```txt
ValidationBehavior
LoggingBehavior
```

Flow:

```txt
Endpoint
  ↓
MediatR
  ↓
ValidationBehavior
  ↓
LoggingBehavior
  ↓
Handler
```

## IApplicationDbContext

Application defines the database abstraction:

```csharp
public interface IApplicationDbContext
{
    DbSet<Todo> Todos { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

This allows handlers to use the database without depending directly on Infrastructure.
