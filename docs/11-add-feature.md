# 11 - Adding a New Feature

This guide shows how to add a new feature using the template style.

Example feature:

```txt
Budgets
```

## 1. Add Domain Entity

```txt
Project.Domain/
└── Budgets/
    ├── Budget.cs
    └── BudgetErrors.cs
```

## 2. Add Application Feature

```txt
Project.Application/
└── Features/
    └── Budgets/
        └── CreateBudget/
            ├── CreateBudgetCommand.cs
            ├── CreateBudgetHandler.cs
            ├── CreateBudgetValidator.cs
            └── CreateBudgetResponse.cs
```

## 3. Add Infrastructure Configuration

```txt
Project.Infrastructure/
└── Persistence/
    └── Configurations/
        └── BudgetConfiguration.cs
```

## 4. Add DbSet to IApplicationDbContext

```csharp
DbSet<Budget> Budgets { get; }
```

## 5. Add DbSet to ApplicationDbContext

```csharp
public DbSet<Budget> Budgets => Set<Budget>();
```

## 6. Add Endpoint

```txt
Project.API/
└── Endpoints/
    └── Budgets/
        ├── BudgetEndpoints.cs
        └── CreateBudgetRequest.cs
```

## 7. Implement IEndpoint

```csharp
public sealed class BudgetEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/budgets")
            .WithTags("Budgets");

        group.MapPost("/", CreateBudget);
    }

    private static async Task<IResult> CreateBudget(
        CreateBudgetRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new CreateBudgetCommand(request.Name);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            budget => Results.Created($"/api/budgets/{budget.Id}", budget),
            errors => errors.ToProblem());
    }
}
```

Because endpoint registration is automatic, you do not need to edit `Program.cs`.

## 8. Add Migration

```bash
dotnet ef migrations add AddBudgets \
  --project src/Project.Infrastructure/Project.Infrastructure.csproj \
  --startup-project src/Project.API/Project.API.csproj \
  --output-dir Persistence/Migrations
```

## 9. Apply Migration

```bash
dotnet ef database update \
  --project src/Project.Infrastructure/Project.Infrastructure.csproj \
  --startup-project src/Project.API/Project.API.csproj
```

## Feature Checklist

For every new feature, ask:

```txt
Did I add the Domain entity?
Did I add the Application command/query?
Did I add the handler?
Did I add the validator?
Did I add the EF configuration?
Did I update IApplicationDbContext?
Did I update ApplicationDbContext?
Did I add an endpoint class?
Did I add a migration?
Did I add tests?
```

