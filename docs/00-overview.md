# 00 - Overview

This template is a starting point for building .NET Minimal API projects.

It uses:

- Vertical Slice Architecture
- Lightweight Clean Architecture boundaries
- CQRS
- MediatR
- FluentValidation
- ErrorOr
- EF Core
- PostgreSQL
- Docker
- Serilog
- Seq
- Scalar
- xUnit
- Testcontainers
- Architecture tests
- `dotnet new` template support

The SDK is pinned by `global.json` to .NET `10.0.100` with `latestFeature` roll-forward. Package versions are managed centrally in `Directory.Packages.props`.

The goal is to stay simple while keeping good project boundaries.

## Template Philosophy

This template avoids:

- generic repositories by default
- too many abstractions
- over-layered Clean Architecture ceremony
- controllers
- separate read/write databases
- unnecessary shared projects

It keeps:

- thin Minimal API endpoints
- feature-based Application layer
- pure Domain layer
- Infrastructure for database/external services
- real PostgreSQL integration tests
- architecture tests

## Main Idea

The template follows this flow:

```txt
HTTP Request
    ↓
Minimal API Endpoint
    ↓
MediatR Command/Query
    ↓
Application Handler
    ↓
Domain Entity / Business Rule
    ↓
EF Core / PostgreSQL
    ↓
ErrorOr Result
    ↓
HTTP Response
```

The most important rule:

> Organize by feature, but keep dependency direction clean.

## Current Sample Feature

The repository ships with a Todo feature that demonstrates the pattern:

```txt
POST /api/todos
GET  /api/todos/{id}
```

The sample includes a Domain entity, Application commands/queries, EF Core configuration, Minimal API endpoints, unit tests, integration tests, and architecture tests.
