# VSA .NET Minimal API Template

A simple, production-minded .NET Minimal API template using Vertical Slice Architecture with lightweight Clean Architecture boundaries.

## Stack

- .NET 10
- ASP.NET Core Minimal APIs
- Vertical Slice Architecture
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

## Architecture

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

## Project Structure

```txt
src/
├── Project.API
├── Project.Application
├── Project.Domain
├── Project.Infrastructure
└── Project.slnx

tests/
├── Project.UnitTests
├── Project.IntegrationTests
└── Project.ArchitectureTests
```

## Getting Started

### 1. Copy environment file

```bash
cp .env.example .env
```

### 2. Start local services

```bash
docker compose up -d postgres seq
```

### 3. Restore and build

```bash
dotnet restore src/Project.slnx
dotnet build src/Project.slnx
```

### 4. Run migrations

```bash
dotnet ef database update \
  --project src/Project.Infrastructure/Project.Infrastructure.csproj \
  --startup-project src/Project.API/Project.API.csproj
```

### 5. Run API

```bash
dotnet run --project src/Project.API/Project.API.csproj
```

Default local API URL:

```txt
http://localhost:5014
```

Health checks:

```txt
GET /health/live
GET /health/ready
```

Scalar API docs:

```txt
http://localhost:5014/scalar/v1
```

OpenAPI document:

```txt
http://localhost:5014/openapi/v1.json
```

## Example: Create Todo

```bash
curl -X POST http://localhost:5014/api/todos \
  -H "Content-Type: application/json" \
  -d '{"title":"Learn VSA template"}'
```

## Run Tests

```bash
dotnet test src/Project.slnx
```

## Rename Template

```bash
./rename-template.sh YourProjectName
```

Example:

```bash
./rename-template.sh Tigum
```

## Architecture Rules

- Domain must not depend on Application, Infrastructure, or API.
- Application must not depend on Infrastructure or API.
- Infrastructure must not depend on API.
- API is the composition root.

## Template Philosophy

This template is intentionally simple.

It avoids:

- Generic repositories by default
- Over-layered Clean Architecture ceremony
- Separate read/write databases
- Excessive abstractions
- Controller-based API structure

It keeps:

- Clean project boundaries
- Feature-based Application layer
- Thin Minimal API endpoints
- Real PostgreSQL integration tests
- Centralized package management
- Built-in health checks
- Docker-based local infrastructure

## Local Development Workflow

Run infrastructure only:

```bash
docker compose up -d postgres seq
```

Run the API locally:

```bash
dotnet run --project src/Project.API/Project.API.csproj
```

Test the health endpoint:

```bash
curl http://localhost:5014/health/ready
```

## Full Docker Workflow

Run everything with Docker Compose:

```bash
docker compose up -d
```

API URL:

```txt
http://localhost:8080
```

Health check:

```bash
curl http://localhost:8080/health/ready
```

## EF Core Commands

Add a migration:

```bash
dotnet ef migrations add MigrationName \
  --project src/Project.Infrastructure/Project.Infrastructure.csproj \
  --startup-project src/Project.API/Project.API.csproj \
  --output-dir Persistence/Migrations
```

Apply migrations:

```bash
dotnet ef database update \
  --project src/Project.Infrastructure/Project.Infrastructure.csproj \
  --startup-project src/Project.API/Project.API.csproj
```

Remove last migration:

```bash
dotnet ef migrations remove \
  --project src/Project.Infrastructure/Project.Infrastructure.csproj \
  --startup-project src/Project.API/Project.API.csproj
```
