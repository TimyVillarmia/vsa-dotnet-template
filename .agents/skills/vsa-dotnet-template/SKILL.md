---

name: vsa-dotnet-template
description: Work on this .NET 10 Minimal API VSA template. Use when adding features, fixing tests, updating template.json, editing endpoint registration, working with EF Core migrations, Docker Compose, Testcontainers, Scalar, Serilog, Seq, or explaining how this template works.
---

# VSA .NET Template Skill

Use this skill when working inside this repository or a project generated from it.

This template is a .NET 10 Minimal API template using:

* Vertical Slice Architecture
* Lightweight Clean Architecture boundaries
* CQRS
* MediatR
* FluentValidation
* ErrorOr
* EF Core
* PostgreSQL
* Docker Compose
* Serilog
* Seq
* Scalar
* xUnit
* Testcontainers
* Architecture tests
* `dotnet new` template support

## Core architecture

The dependency direction must stay:

```txt
API
├── Application
└── Infrastructure

Application
└── Domain

Infrastructure
├── Application
└── Domain

Domain
└── nothing
```

Do not introduce dependencies that violate this.

## Project naming

The template placeholder is:

```txt
VsaTemplate
```

Generated projects use:

```bash
dotnet new vsa-api -n MyApp
```

Do not use a shell rename script. This repository should rely on `.template.config/template.json`.

## Important gotchas

* Do not globally replace the word `Project`. It breaks MSBuild XML such as `<Project Sdk="Microsoft.NET.Sdk.Web">`.
* In `launchSettings.json`, `"commandName": "Project"` is correct. Do not replace it with the app name.
* `sourceName` in `template.json` should be `VsaTemplate`.
* Template parameters need `"replaces"` to actually replace tokens like `databaseName`, `seqPort`, and `localApiPort`.
* Integration tests must not use local Docker Compose ports like `5432` or `5433`. They must use the Testcontainers connection string.
* In integration tests, override `ApplicationDbContext` registration with `_postgres.GetConnectionString()`.
* Local app config can use `Host=localhost`.
* Docker app config should use `Host=postgres`.
* Docker Compose service names are used inside the Docker network.
* Keep endpoints thin. Business logic goes in Application/Domain.
* Keep EF Core configuration in Infrastructure.
* Do not add generic repositories by default.

## Standard commands

Restore:

```bash
dotnet restore src/VsaTemplate.slnx
```

Build:

```bash
dotnet build src/VsaTemplate.slnx
```

Test:

```bash
dotnet test src/VsaTemplate.slnx
```

Run API:

```bash
dotnet run --project src/VsaTemplate.API/VsaTemplate.API.csproj
```

Start local infrastructure:

```bash
cp .env.example .env
docker compose up -d postgres seq
```

Apply migrations:

```bash
dotnet ef database update \
  --project src/VsaTemplate.Infrastructure/VsaTemplate.Infrastructure.csproj \
  --startup-project src/VsaTemplate.API/VsaTemplate.API.csproj
```

Add migration:

```bash
dotnet ef migrations add MigrationName \
  --project src/VsaTemplate.Infrastructure/VsaTemplate.Infrastructure.csproj \
  --startup-project src/VsaTemplate.API/VsaTemplate.API.csproj \
  --output-dir Persistence/Migrations
```

## Template validation workflow

After editing `.template.config/template.json` or placeholder tokens:

```bash
dotnet new uninstall .
dotnet new install .
```

Generate a test project outside the repo:

```bash
mkdir -p ~/projects/template-test
cd ~/projects/template-test
rm -rf MyApp

dotnet new vsa-api -n MyApp \
  --databaseName myapp_db \
  --postgresUser postgres \
  --postgresPassword postgres \
  --postgresPort 5433 \
  --seqPort 5342 \
  --apiPort 8081 \
  --localApiPort 5015 \
  --localHttpsPort 7049
```

Validate:

```bash
cd MyApp

dotnet restore src/MyApp.slnx
dotnet build src/MyApp.slnx
dotnet test src/MyApp.slnx
```

Check that template tokens were replaced:

```bash
grep -R "VsaTemplate\|databaseName\|postgresUser\|postgresPassword\|postgresPort\|seqPort\|seqPassword\|apiPort\|localApiPort\|localHttpsPort" -n . \
  --exclude-dir=.git \
  --exclude-dir=bin \
  --exclude-dir=obj
```

Expected: no important source/config references remain.

## Adding a new feature

Use this order:

1. Add Domain entity and errors.
2. Add Application command/query folder.
3. Add command/query, handler, validator, and response.
4. Add DbSet to `IApplicationDbContext`.
5. Add DbSet to `ApplicationDbContext`.
6. Add EF configuration in Infrastructure.
7. Add endpoint request model in API.
8. Add endpoint class implementing `IEndpoint`.
9. Add migration.
10. Add or update tests.

Feature folder pattern:

```txt
src/VsaTemplate.Application/Features/{FeatureName}/{UseCase}/
├── {UseCase}Command.cs
├── {UseCase}Handler.cs
├── {UseCase}Validator.cs
└── {UseCase}Response.cs
```

Endpoint pattern:

```txt
src/VsaTemplate.API/Endpoints/{FeatureName}/
├── {FeatureName}Endpoints.cs
└── {RequestName}.cs
```

## Endpoint registration

Endpoints use `IEndpoint` and `EndpointExtensions`.

Every endpoint group should implement:

```csharp
public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
```

`EndpointExtensions.AddEndpoints()` scans the API assembly for `IEndpoint` implementations and registers them in DI.

`EndpointExtensions.MapEndpoints()` resolves all `IEndpoint` instances and calls `MapEndpoint(app)`.

Do not manually add every endpoint in `Program.cs`.

Correct:

```csharp
app.MapEndpoints();
```

Avoid:

```csharp
app.MapTodoEndpoints();
app.MapBudgetEndpoints();
app.MapAuthEndpoints();
```

## Test strategy

Run all tests before finishing changes:

```bash
dotnet test src/VsaTemplate.slnx
```

Test projects:

```txt
tests/VsaTemplate.UnitTests
tests/VsaTemplate.IntegrationTests
tests/VsaTemplate.ArchitectureTests
```

When changing Domain behavior, add/update unit tests.

When changing HTTP behavior, add/update integration tests.

When changing project references or architecture boundaries, run architecture tests.

## Integration test requirements

Integration tests should use Testcontainers.

The test `ApiFactory` must:

1. Start a PostgreSQL container.
2. Override app configuration with `_postgres.GetConnectionString()`.
3. Remove existing `ApplicationDbContext` registrations.
4. Re-register `ApplicationDbContext` using the Testcontainers connection string.
5. Run migrations before tests.

Use this pattern:

```csharp
services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
services.RemoveAll<ApplicationDbContext>();
services.RemoveAll<IApplicationDbContext>();

services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(_postgres.GetConnectionString());
});

services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<ApplicationDbContext>());
```

## Documentation

Main docs are split by concept:

```txt
docs/00-overview.md
docs/01-architecture.md
docs/02-request-flow.md
docs/03-endpoints.md
docs/04-application-layer.md
docs/05-domain-layer.md
docs/06-infrastructure.md
docs/07-configuration.md
docs/08-health-logging.md
docs/09-migrations.md
docs/10-testing.md
docs/11-add-feature.md
```

Read the relevant doc before making broad changes.

## Before final response

Before saying the work is done:

1. Run restore/build/test when code changed.
2. For template changes, generate a sample project and test it.
3. Mention any command that was not run.
4. Mention any failing tests or warnings.
