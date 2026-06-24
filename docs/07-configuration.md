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
POSTGRES_DB=databaseName
POSTGRES_USER=postgresUser
POSTGRES_PASSWORD=postgresPassword
POSTGRES_PORT=postgresPort

SEQ_PORT=seqPort
SEQ_PASSWORD=seqPassword

API_PORT=apiPort
ASPNETCORE_ENVIRONMENT=Development

ConnectionStrings__Database=Host=postgres;Port=5432;Database=databaseName;Username=postgresUser;Password=postgresPassword
Serilog__WriteTo__1__Args__serverUrl=http://seq:80
```

Those placeholder values are replaced when the template is generated with `dotnet new vsa-api`.

## .env

`.env` is local only.

It should be ignored by Git.

Create it with:

```bash
cp .env.example .env
```

## appsettings.json

This is app configuration.

In the template source, local `dotnet run` configuration uses replaceable placeholders:

```json
{
  "ConnectionStrings": {
    "Database": "Host=localhost;Port=postgresPort;Database=databaseName;Username=postgresUser;Password=postgresPassword"
  }
}
```

After `dotnet new vsa-api`, those placeholders are replaced with the selected template parameter values.

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

## Template Parameters

`.template.config/template.json` defines these replaceable parameters:

```txt
databaseName
postgresUser
postgresPassword
postgresPort
seqPort
seqPassword
apiPort
localApiPort
localHttpsPort
```

Use them when creating a new project:

```bash
dotnet new vsa-api -n MyApp --databaseName myapp_db --apiPort 8081
```
