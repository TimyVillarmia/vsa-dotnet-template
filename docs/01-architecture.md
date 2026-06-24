# 01 - Architecture

The template uses Vertical Slice Architecture with lightweight Clean Architecture boundaries.

## Project Structure

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
```

Shared repository-level build files:

```txt
global.json
Directory.Build.props
Directory.Packages.props
docker-compose.yml
docker-compose.override.yml
.template.config/template.json
```

## Dependency Direction

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

Visual:

```txt
API  ───────────────┐
 │                  │
 ▼                  ▼
Application ◄── Infrastructure
 │
 ▼
Domain
```

## Layer Rules

### Domain

Domain should not depend on anything.

It should not know about:

* ASP.NET Core
* EF Core
* PostgreSQL
* MediatR
* Serilog
* API endpoints

### Application

Application can depend on Domain.

Application contains:

* commands
* queries
* handlers
* validators
* application abstractions
* MediatR behaviors

Application should not depend on Infrastructure or API.

### Infrastructure

Infrastructure can depend on Application and Domain.

Infrastructure contains:

* DbContext
* EF Core entity configuration
* migrations
* external service implementations
* database health checks

### API

API is the composition root.

API wires everything together:

* Presentation services
* Application services
* Infrastructure services
* middleware
* endpoints
* health checks
* Scalar docs

## Build Conventions

Projects target `net10.0` with nullable reference types and implicit usings enabled.

Package versions are centralized in `Directory.Packages.props`, so project files use package names without explicit versions.
