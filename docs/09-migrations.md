# 09 - EF Core Migrations

Migrations live in Infrastructure:

```txt
VsaTemplate.Infrastructure/
└── Persistence/
    └── Migrations/
```

The current repository includes an initial migration:

```txt
20260623075831_InitialCreate
```

## Add Migration

```bash
dotnet ef migrations add InitialCreate \
  --project src/VsaTemplate.Infrastructure/VsaTemplate.Infrastructure.csproj \
  --startup-project src/VsaTemplate.API/VsaTemplate.API.csproj \
  --output-dir Persistence/Migrations
```

## Apply Migration

```bash
dotnet ef database update \
  --project src/VsaTemplate.Infrastructure/VsaTemplate.Infrastructure.csproj \
  --startup-project src/VsaTemplate.API/VsaTemplate.API.csproj
```

## Remove Last Migration

```bash
dotnet ef migrations remove \
  --project src/VsaTemplate.Infrastructure/VsaTemplate.Infrastructure.csproj \
  --startup-project src/VsaTemplate.API/VsaTemplate.API.csproj
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
--project         = VsaTemplate.Infrastructure
--startup-project = VsaTemplate.API
```

## Migration History

EF Core tracks applied migrations in:

```txt
__EFMigrationsHistory
```

Check tables:

```bash
docker exec -it vsatemplate-postgres psql -U postgres -d vsatemplate_db -c "\dt"
```

Check migrations:

```bash
docker exec -it vsatemplate-postgres psql -U postgres -d vsatemplate_db -c 'SELECT * FROM "__EFMigrationsHistory";'
```
