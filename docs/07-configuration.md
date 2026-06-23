# 07 - Configuration

This template uses:

```txt
.env.example
.env
docker-compose.yml
docker-compose.override.yml
appsettings.json
appsettings.Development.json
```

## .env.example

`.env.example` is committed to Git.

It contains sample values.

Example:

```env
POSTGRES_DB=vsatemplate_db
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres
POSTGRES_PORT=5432

SEQ_PORT=5341
SEQ_PASSWORD=admin123

API_PORT=8080
ASPNETCORE_ENVIRONMENT=Development

ConnectionStrings__Database=Host=postgres;Port=5432;Database=vsatemplate_db;Username=postgres;Password=postgres
Serilog__WriteTo__1__Args__serverUrl=http://seq:80
```

## .env

`.env` is local only.

It should be ignored by Git.

Create it with:

```bash
cp .env.example .env
```

## appsettings.json

This is app configuration.

For local `dotnet run`, use:

```json
{
  "ConnectionStrings": {
    "Database": "Host=localhost;Port=5432;Database=vsatemplate_db;Username=postgres;Password=postgres"
  }
}
```

## Environment Variable Overrides

ASP.NET Core supports double underscore for nested configuration.

This:

```env
ConnectionStrings__Database=Host=postgres;Port=5432;Database=vsatemplate_db;Username=postgres;Password=postgres
```

maps to:

```json
{
  "ConnectionStrings": {
    "Database": "Host=postgres;Port=5432;Database=vsatemplate_db;Username=postgres;Password=postgres"
  }
}
```

## Local API + Docker Infrastructure

Use this when running API with `dotnet run`.

```bash
docker compose up -d postgres seq
dotnet run --project src/VsaTemplate.API/VsaTemplate.API.csproj
```

Connection string:

```txt
Host=localhost
```

## Full Docker

Use this when API also runs inside Docker.

```bash
docker compose up -d
```

Connection string:

```txt
Host=postgres
```

Because `postgres` is the Docker Compose service name.
