# 03 - Endpoints

Endpoints live in:

```txt
VsaTemplate.API/Endpoints
```

Example:

```txt
VsaTemplate.API/
└── Endpoints/
    ├── IEndpoint.cs
    ├── EndpointExtensions.cs
    └── Todos/
        ├── TodoEndpoints.cs
        ├── CreateTodoRequest.cs
        └── UpdateTodoRequest.cs
```

## Purpose of Endpoints

Endpoints are responsible for:

* receiving HTTP requests
* mapping HTTP request models to commands/queries
* sending commands/queries through MediatR
* mapping results to HTTP responses

Endpoints should not contain business logic.

## IEndpoint

```csharp
namespace VsaTemplate.API.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
```

Every endpoint group implements `IEndpoint`.

Example:

```csharp
public sealed class TodoEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/todos")
            .WithTags("Todos");

        group.MapPost("/", CreateTodo);
        group.MapGet("/{id:guid}", GetTodoById);
    }
}
```

## Why EndpointExtensions Exists

Without `EndpointExtensions.cs`, you would need to manually register endpoints in `Program.cs`:

```csharp
app.MapTodoEndpoints();
app.MapAuthEndpoints();
app.MapUserEndpoints();
app.MapBudgetEndpoints();
```

That works, but it becomes repetitive.

This template uses automatic endpoint registration.

## EndpointExtensions.cs

```csharp
using System.Reflection;

namespace VsaTemplate.API.Endpoints;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        var endpointTypes = Assembly.GetExecutingAssembly()
            .DefinedTypes
            .Where(type =>
                !type.IsAbstract &&
                !type.IsInterface &&
                typeof(IEndpoint).IsAssignableFrom(type));

        foreach (var endpointType in endpointTypes)
        {
            services.AddTransient(typeof(IEndpoint), endpointType);
        }

        return services;
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }

        return app;
    }
}
```

## How AddEndpoints Works

This scans the current API assembly:

```csharp
Assembly.GetExecutingAssembly().DefinedTypes
```

Then it finds all classes that implement `IEndpoint`.

It ignores:

* interfaces
* abstract classes
* unrelated classes

Then it registers each endpoint class in dependency injection:

```csharp
services.AddTransient(typeof(IEndpoint), endpointType);
```

## How MapEndpoints Works

This gets all registered endpoints:

```csharp
var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
```

Then it calls:

```csharp
endpoint.MapEndpoint(app);
```

for each endpoint.

So this:

```csharp
app.MapEndpoints();
```

automatically maps all endpoint groups.

## Adding a New Endpoint Group

Create a class:

```csharp
public sealed class BudgetEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/budgets")
            .WithTags("Budgets");

        group.MapGet("/", GetBudgets);
    }

    private static IResult GetBudgets()
    {
        return Results.Ok();
    }
}
```

Because it implements `IEndpoint`, it is discovered automatically.

No need to edit `Program.cs`.
