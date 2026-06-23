# 02 - Request Flow

This document explains what happens when a request enters the API.

Example request:

```txt
POST /api/todos
```

## Full Flow

```txt
HTTP request
    ↓
TodoEndpoints.CreateTodo
    ↓
CreateTodoCommand
    ↓
MediatR
    ↓
ValidationBehavior
    ↓
LoggingBehavior
    ↓
CreateTodoHandler
    ↓
Todo.Create()
    ↓
ApplicationDbContext
    ↓
PostgreSQL
    ↓
ErrorOr<CreateTodoResponse>
    ↓
HTTP response
```

## Step-by-Step

### 1. Endpoint receives HTTP request

The endpoint receives the request body:

```json
{
  "title": "Learn VSA template"
}
```

The API model is:

```csharp
public sealed record CreateTodoRequest(string Title);
```

### 2. Endpoint creates command

```csharp
var command = new CreateTodoCommand(request.Title);
```

### 3. Endpoint sends command through MediatR

```csharp
var result = await sender.Send(command, cancellationToken);
```

### 4. ValidationBehavior runs

The command is validated before the handler runs.

### 5. LoggingBehavior runs

The request name is logged.

Example:

```txt
Handling request CreateTodoCommand
Handled request CreateTodoCommand
```

### 6. Handler executes use case

The handler creates a Todo and saves it.

### 7. Domain validates business rules

```csharp
var todoResult = Todo.Create(request.Title);
```

The Domain decides whether the Todo is valid.

### 8. Infrastructure saves to PostgreSQL

EF Core saves the entity.

### 9. Result is returned

The handler returns:

```csharp
ErrorOr<CreateTodoResponse>
```

### 10. Endpoint maps result to HTTP

```csharp
return result.Match(
    todo => Results.Created($"/api/todos/{todo.Id}", todo),
    errors => errors.ToProblem());
```