# AGENTS.md

This file mirrors the repository-level agent guidance for clients that inspect nested agent instructions. Prefer the root `AGENTS.md` when both are available.

## Project Overview

This repository is a reusable `.NET 10` Minimal API template that uses Vertical Slice Architecture with lightweight Clean Architecture boundaries.

Install and use it with:

```bash
dotnet new install .
dotnet new vsa-api -n MyApp
```

The template placeholder is `VsaTemplate`.

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
- Keep endpoints thin and put use-case logic in Application handlers.
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

## Endpoints

Endpoints live under `src/VsaTemplate.API/Endpoints`.

Every endpoint group implements `IEndpoint`. `EndpointExtensions.AddEndpoints()` scans the API assembly and registers endpoint groups. `app.MapEndpoints()` maps them at startup.

Do not manually add every endpoint group in `Program.cs`.

Current sample routes:

```txt
POST /api/todos
GET  /api/todos/{id:guid}
```

`UpdateTodoRequest.cs` exists as a request model, but no update route is currently mapped.

## Testing

- Domain behavior changes need unit tests.
- HTTP behavior changes need integration tests.
- Project reference or layer-boundary changes need architecture tests.
- Integration tests use Testcontainers PostgreSQL and must override `ApplicationDbContext` with `_postgres.GetConnectionString()`.
- Run `dotnet test src/VsaTemplate.slnx` before finishing code changes when feasible.

## Docs

Read the relevant docs before broad changes:

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
