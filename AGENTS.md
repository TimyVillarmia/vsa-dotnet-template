# AGENTS.md

## Project Overview

This repository is a reusable `.NET 10` Minimal API template that uses Vertical Slice Architecture with lightweight Clean Architecture boundaries.

It can be used directly as a starter repository or installed as a `dotnet new` template:

```bash
dotnet new install .
dotnet new vsa-api -n MyApp
```

The template placeholder is `VsaTemplate`. Generated projects replace that name with the requested project name.

## Stack

- .NET 10
- ASP.NET Core Minimal APIs
- CQRS with MediatR
- FluentValidation
- ErrorOr
- EF Core and PostgreSQL
- Docker Compose
- Serilog and Seq
- Scalar API docs
- xUnit, Testcontainers, and architecture tests
- Central Package Management
- `dotnet new` template configuration

## Repository Layout

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
```

## Architecture Rules

Dependency direction must stay:

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

- Domain must not depend on Application, Infrastructure, API, EF Core, ASP.NET Core, or MediatR.
- Application may depend on Domain only.
- Infrastructure may depend on Application and Domain, but not API.
- API is the composition root.
- Keep endpoints thin; use Application handlers for use-case logic.
- Keep business rules in Domain.
- Keep EF Core configuration, migrations, and external service implementations in Infrastructure.
- Do not add generic repositories by default.

## Commands

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

## EF Core

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

## Endpoint Rules

Endpoints live under `src/VsaTemplate.API/Endpoints`.

Every endpoint group implements `IEndpoint`. `EndpointExtensions.AddEndpoints()` scans the API assembly and registers endpoint groups. `app.MapEndpoints()` maps them at startup.

Do not manually add every endpoint group in `Program.cs`.

Current sample routes:

```txt
POST /api/todos
GET  /api/todos/{id:guid}
```

`UpdateTodoRequest.cs` exists as a request model, but no update route is currently mapped.

## Adding Features

Follow the repository pattern:

1. Add Domain entity and errors.
2. Add Application command/query folder.
3. Add command/query, handler, validator, and response.
4. Add or update `IApplicationDbContext`.
5. Add or update `ApplicationDbContext`.
6. Add EF Core configuration in Infrastructure.
7. Add endpoint request model in API.
8. Add endpoint class implementing `IEndpoint`.
9. Add migration when persistence changes.
10. Add or update tests.

Read `docs/11-add-feature.md` before broad feature work.

## Template Rules

Template configuration lives in `.template.config/template.json`.

- `sourceName` must remain `VsaTemplate`.
- Template parameter symbols must include `"replaces"` when they substitute tokens.
- Current replaceable parameters: `databaseName`, `postgresUser`, `postgresPassword`, `postgresPort`, `seqPort`, `seqPassword`, `apiPort`, `localApiPort`, `localHttpsPort`.
- Do not use shell scripts to rename the template; rely on `.template.config/template.json`.
- Do not globally replace the word `Project`; it breaks MSBuild XML and `launchSettings.json`.
- In `launchSettings.json`, `"commandName": "Project"` is correct.
- Do not use local Docker Compose ports in integration tests; use Testcontainers connection strings.

Validate template changes by installing the template, generating a project outside this repository, then restoring, building, and testing the generated project.

## Testing Guidance

- Domain behavior changes need unit tests.
- HTTP behavior changes need integration tests.
- Project reference or layer-boundary changes need architecture tests.
- Integration tests use Testcontainers PostgreSQL and must override `ApplicationDbContext` with `_postgres.GetConnectionString()`.
- Run `dotnet test src/VsaTemplate.slnx` before finishing code changes when feasible.

## Code Style

- Nullable reference types and implicit usings are enabled.
- Use file-scoped namespaces.
- Prefer `sealed` classes when inheritance is not intended.
- Prefer records for request, response, command, and query models.
- Use async database and HTTP APIs.
- Pass `CancellationToken` through endpoints and handlers.
- Keep one use case per feature folder.
