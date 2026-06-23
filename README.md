# VSA .NET Minimal API Template

A simple .NET Minimal API template using Vertical Slice Architecture with lightweight Clean Architecture boundaries.

## Stack

- .NET 10
- ASP.NET Core Minimal APIs
- CQRS
- MediatR
- FluentValidation
- ErrorOr
- EF Core
- PostgreSQL
- Serilog
- Seq
- Scalar
- Docker Compose
- xUnit
- Testcontainers
- Architecture tests

## Quick Start

Copy environment file:

```bash
cp .env.example .env
```

Start infrastructure:

```bash
docker compose up -d postgres seq
```

Restore and build:

```bash
dotnet restore src/VsaTemplate.slnx
dotnet build src/VsaTemplate.slnx
```

Apply database migrations:

```bash
dotnet ef database update \
  --project src/VsaTemplate.Infrastructure/VsaTemplate.Infrastructure.csproj \
  --startup-project src/VsaTemplate.API/VsaTemplate.API.csproj
```

Run API:

```bash
dotnet run --project src/VsaTemplate.API/VsaTemplate.API.csproj
```

API URL:

```txt
http://localhost:5014
```

Scalar docs:

```txt
http://localhost:5014/scalar/v1
```

Health checks:

```txt
GET /health/live
GET /health/ready
```

Run tests:

```bash
dotnet test src/VsaTemplate.slnx
```

## Documentation

Read these in order:

1. [Overview](docs/00-overview.md)
2. [Architecture](docs/01-architecture.md)
3. [Request Flow](docs/02-request-flow.md)
4. [Endpoints](docs/03-endpoints.md)
5. [Application Layer](docs/04-application-layer.md)
6. [Domain Layer](docs/05-domain-layer.md)
7. [Infrastructure](docs/06-infrastructure.md)
8. [Configuration](docs/07-configuration.md)
9. [Health, Logging, and API Docs](docs/08-health-logging.md)
10. [Migrations](docs/09-migrations.md)
11. [Testing](docs/10-testing.md)
12. [Adding a Feature](docs/11-add-feature.md)

```
````
