# 10 - Testing

This template has three test projects:

```txt
tests/
├── VsaTemplate.UnitTests
├── VsaTemplate.IntegrationTests
└── VsaTemplate.ArchitectureTests
```

## Unit Tests

Unit tests test small pieces of logic.

Example:

```txt
VsaTemplate.UnitTests/Domain/Todos/TodoTests.cs
```

Unit tests should not require:

* database
* HTTP server
* Docker
* external services

Example:

```csharp
[Fact]
public void Create_ShouldReturnError_WhenTitleIsEmpty()
{
    var result = Todo.Create("");

    Assert.True(result.IsError);
    Assert.Contains(result.Errors, error => error.Code == "Todo.TitleRequired");
}
```

## Integration Tests

Integration tests test real behavior.

This template uses Testcontainers to run PostgreSQL during tests.

Flow:

```txt
HTTP request
→ Minimal API
→ MediatR
→ EF Core
→ PostgreSQL container
→ HTTP response
```

## Architecture Tests

Architecture tests protect project boundaries.

Rules:

```txt
Domain must not depend on Application
Domain must not depend on Infrastructure
Domain must not depend on API

Application must not depend on Infrastructure
Application must not depend on API

Infrastructure must not depend on API
```

Example mistake:

```csharp
using VsaTemplate.Infrastructure.Persistence;
```

inside Application should fail architecture tests.

## Run All Tests

```bash
dotnet test src/VsaTemplate.slnx
```