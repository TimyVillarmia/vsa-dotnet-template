# 09 - EF Core Migrations

Migrations live in Infrastructure:

```txt
Project.Infrastructure/
└── Persistence/
    └── Migrations/
```

## Add Migration

```bash
dotnet ef migrations add InitialCreate \
  --project src/Project.Infrastructure/Project.Infrastructure.csproj \
  --startup-project src/Project.API/Project.API.csproj \
  --output-dir Persistence/Migrations
```

## Apply Migration

```bash
dotnet ef database update \
  --project src/Project.Infrastructure/Project.Infrastructure.csproj \
  --startup-project src/Project.API/Project.API.csproj
```

## Remove Last Migration

```bash
dotnet ef migrations remove \
  --project src/Project.Infrastructure/Project.Infrastructure.csproj \
  --startup-project src/Project.API/Project.API.csproj
```

## Why Use --project and --startup-project?

```txt
--project
The project where DbContext and migrations live.

--startup-project
The project used to run the app and load configuration.
```

In this template:

```txt
--project         = Project.Infrastructure
--startup-project = Project.API
```

## Migration History

EF Core tracks applied migrations in:

```txt
__EFMigrationsHistory
```

Check tables:

```bash
docker exec -it project-postgres psql -U postgres -d project_db -c "\dt"
```

Check migrations:

```bash
docker exec -it project-postgres psql -U postgres -d project_db -c 'SELECT * FROM "__EFMigrationsHistory";'
```
