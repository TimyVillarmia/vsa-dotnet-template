# 08 - Health, Logging, and API Docs

This template includes:

- built-in health checks
- Serilog
- Seq
- Scalar
- OpenAPI

## Health Checks

Endpoints:

```txt
GET /health/live
GET /health/ready
```

### /health/live

Checks whether the app process is alive.

It does not check the database.

### /health/ready

Checks whether the app is ready to serve real traffic.

It includes the database check.

## Health Check Mapping

```csharp
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});
```

## Database Health Check

Registered in Infrastructure:

```csharp
services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>(
        name: "database",
        tags: ["ready"]);
```

## Serilog

Serilog is configured in `Program.cs`:

```csharp
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});
```

## Seq

Seq is used for local structured logs.

Local URL:

```txt
http://localhost:5341
```

Docker service URL from API container:

```txt
http://seq:80
```

## Scalar

Scalar provides API documentation UI.

Scalar URL:

```txt
http://localhost:5014/scalar/v1
```

OpenAPI JSON:

```txt
http://localhost:5014/openapi/v1.json
```