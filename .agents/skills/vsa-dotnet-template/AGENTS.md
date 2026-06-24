# AGENTS.md

## Project overview

This repository is a reusable `.NET 10` project template for building Minimal API applications with Vertical Slice Architecture and lightweight Clean Architecture boundaries.

It is installed and used with:

```bash
dotnet new install .
dotnet new vsa-api -n MyApp
```

The template placeholder name is:

```txt
VsaTemplate
```

The generated application name replaces `VsaTemplate`.

## Stack

* .NET 10
* ASP.NET Core Minimal APIs
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
* NetArchTest
* Central Package Management
* `dotnet new` template configuration

## Repository layout

```txt
src/
├── VsaTemplate.API
├── VsaTemplate.Application
├── VsaTemplate.Domain
├── VsaTemplate.Infrastructure
└── VsaTemplate.slnx

tests/
├── VsaTemplate.UnitTests
├── VsaTemplate.IntegrationTests
└── VsaTemplate.ArchitectureTests

docs/
├── 00-overview.md
├── 01-architecture.md
├── 02-request-flow.md
├── 03-endpoints.md
├── 04-application-layer.md
├── 05-domain-layer.md
├── 06-infrastructure.md
├── 07-configuration.md
├── 08-health-logging.md
├── 09-migrations.md
├── 10-testing.md
└── 11-add-feature.md

.template.config/
└── template.json
```

## Architecture rules

Dependency direction:

```txt
VsaTemplate.API
├── VsaTemplate.Application
└── VsaTemplate.Infrastructure

VsaTemplate.Application
└── VsaTemplate.Domain

VsaTemplate.Infrastructure
├── VsaTemplate.Application
└── VsaTemplate.Domain

VsaTemplate.Domain
└── nothing
```

Rules:

* Domain must not depend on Application, Infrastructure, API, EF Core, ASP.NET Core, or MediatR.
* Application must not depend on Infrastructure or API.
* Infrastructure must not depend on API.
* API is the composition root.
* Keep endpoints thin.
* Keep use-case logic in Application handlers.
* Keep business rules in Domain.
* Keep EF Core and external service implementations in Infrastructure.
* Do not add generic repositories by default.

## Build and test commands

Restore:

```bash
dotnet restore src/VsaTemplate.slnx
```

Build:

```bash
dotnet build src/VsaTemplate.slnx
```

Run all tests:

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

Health checks:

```bash
curl http://localhost:5014/health/live
curl http://localhost:5014/health/ready
```

## EF Core commands

Add migration:

```bash
dotnet ef migrations add MigrationName \
  --project src/VsaTemplate.Infrastructure/VsaTemplate.Infrastructure.csproj \
  --startup-project src/VsaTemplate.API/VsaTemplate.API.csproj \
  --output-dir Persistence/Migrations
```

Apply migrations:

```bash
dotnet ef database update \
  --project src/VsaTemplate.Infrastructure/VsaTemplate.Infrastructure.csproj \
  --startup-project src/VsaTemplate.API/VsaTemplate.API.csproj
```

Remove last migration:

```bash
dotnet ef migrations remove \
  --project src/VsaTemplate.Infrastructure/VsaTemplate.Infrastructure.csproj \
  --startup-project src/VsaTemplate.API/VsaTemplate.API.csproj
```

## Template commands

Install template locally:

```bash
dotnet new install .
```

Uninstall template locally:

```bash
dotnet new uninstall .
```

Create sample project:

```bash
dotnet new vsa-api -n MyApp
```

Create sample project with custom parameters:

```bash
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

Validate generated project:

```bash
cd MyApp

dotnet restore src/MyApp.slnx
dotnet build src/MyApp.slnx
dotnet test src/MyApp.slnx
```

Check for unreplaced template tokens:

```bash
grep -R "VsaTemplate\|databaseName\|postgresUser\|postgresPassword\|postgresPort\|seqPort\|seqPassword\|apiPort\|localApiPort\|localHttpsPort" -n . \
  --exclude-dir=.git \
  --exclude-dir=bin \
  --exclude-dir=obj
```

## Template configuration rules

The template configuration lives in:

```txt
.template.config/template.json
```

Required behavior:

* `sourceName` must remain `VsaTemplate`.
* Template parameter symbols must include `"replaces"`.
* Do not use shell scripts to rename the template.
* Do not globally replace the word `Project`.

Important gotchas:

* Do not replace XML `<Project>` tags.
* Do not replace `"commandName": "Project"` inside `launchSettings.json`.
* Do not rely on local Docker Compose ports in integration tests.
* Integration tests must use Testcontainers connection strings.

## Code style

Use modern C# style:

* Nullable enabled.
* Implicit usings enabled.
* File-scoped namespaces.
* Prefer `sealed` classes when inheritance is not intended.
* Prefer records for request/response/command/query models.
* Use async APIs for database and HTTP operations.
* Pass `CancellationToken` through handlers and endpoints.
* Keep one use case per feature folder.
* Keep validators beside the command/query they validate.
* Keep endpoint request models in API, not Application.

## Endpoint conventions

Endpoints live in:

```txt
src/VsaTemplate.API/Endpoints
```

Endpoint groups implement:

```csharp
public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
```

Use route groups:

```csharp
var group = app
    .MapGroup("/api/todos")
    .WithTags("Todos");
```

Endpoints should:

1. Receive request data.
2. Create command/query.
3. Send through `ISender`.
4. Map `ErrorOr` result to HTTP response.

Endpoints should not:

* access `ApplicationDbContext` directly
* contain business rules
* perform complex mapping logic
* know infrastructure details

## Application conventions

Application features live in:

```txt
src/VsaTemplate.Application/Features
```

Use this structure:

```txt
Features/Todos/CreateTodo/
├── CreateTodoCommand.cs
├── CreateTodoHandler.cs
├── CreateTodoValidator.cs
└── CreateTodoResponse.cs
```

Command means write/change state.

Query means read state.

Handlers may use `IApplicationDbContext`.

Handlers must not reference Infrastructure classes directly.

## Domain conventions

Domain code lives in:

```txt
src/VsaTemplate.Domain
```

Domain should contain:

* entities
* value objects
* domain errors
* business methods
* invariants

Domain should not contain:

* EF Core attributes
* ASP.NET Core types
* HTTP status codes
* MediatR
* Serilog
* database connection logic

## Infrastructure conventions

Infrastructure code lives in:

```txt
src/VsaTemplate.Infrastructure
```

Infrastructure contains:

* `ApplicationDbContext`
* EF Core configurations
* migrations
* external service implementations
* database health check registration

EF Core configurations go in:

```txt
src/VsaTemplate.Infrastructure/Persistence/Configurations
```

Migrations go in:

```txt
src/VsaTemplate.Infrastructure/Persistence/Migrations
```

## Testing instructions

Run all tests before finishing code changes:

```bash
dotnet test src/VsaTemplate.slnx
```

Testing expectations:

* Domain changes require unit tests.
* Endpoint changes require integration tests.
* Dependency/project-reference changes require architecture tests.
* Template changes require generating and testing a sample project.

Integration tests use Testcontainers and should not require manually running Docker Compose services, though Docker must be available.

## Docker and configuration

Local development:

```bash
docker compose up -d postgres seq
dotnet run --project src/VsaTemplate.API/VsaTemplate.API.csproj
```

Full Docker:

```bash
docker compose up -d
```

Local app config uses:

```txt
Host=localhost
```

Docker app config uses:

```txt
Host=postgres
```

`.env.example` should be committed.

`.env` should not be committed.

## Security and secrets

Do not commit real secrets.

Safe for template defaults:

```txt
postgres
admin123
```

Only as local development placeholders.

Never add production credentials to:

* `.env.example`
* `appsettings.json`
* docs
* tests
* CI logs

## Documentation rules

Keep README focused on quick start.

Put study/explanation material in `docs/`.

When adding a concept, update the relevant docs file instead of making README too long.

Use `~~~` fences in Markdown docs when nesting code fences inside generated documentation blocks.

## Pull request / commit checklist

Before committing:

```bash
dotnet restore src/VsaTemplate.slnx
dotnet build src/VsaTemplate.slnx
dotnet test src/VsaTemplate.slnx
```

For template changes, also run:

```bash
dotnet new uninstall .
dotnet new install .
```

Then generate and test:

```bash
mkdir -p ~/projects/template-test
cd ~/projects/template-test
rm -rf MyApp

dotnet new vsa-api -n MyApp
cd MyApp

dotnet restore src/MyApp.slnx
dotnet build src/MyApp.slnx
dotnet test src/MyApp.slnx
```

If any command was not run, mention it clearly.
