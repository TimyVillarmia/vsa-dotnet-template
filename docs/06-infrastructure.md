# 06 - Infrastructure

Infrastructure contains implementation details.

It lives in:

```txt
VsaTemplate.Infrastructure
```

## Responsibilities

Infrastructure contains:

* EF Core DbContext
* PostgreSQL provider setup
* entity configurations
* migrations
* external service implementations
* database health checks

## Structure

```txt
VsaTemplate.Infrastructure/
├── DependencyInjection.cs
├── Persistence/
│   ├── ApplicationDbContext.cs
│   ├── Configurations/
│   │   └── TodoConfiguration.cs
│   └── Migrations/
└── Time/
```

## ApplicationDbContext

```csharp
public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Todo> Todos => Set<Todo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
```

## Dependency Injection

```csharp
public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
    IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("Database");

    services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseNpgsql(connectionString);
    });

    services.AddScoped<IApplicationDbContext>(provider =>
        provider.GetRequiredService<ApplicationDbContext>());

    services.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>(
            name: "database",
            tags: ["ready"]);

    return services;
}
```

This does three things:

```txt
Registers EF Core DbContext
Maps IApplicationDbContext to ApplicationDbContext
Adds database health check
```

## Entity Configuration

```csharp
public sealed class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("todos");

        builder.HasKey(todo => todo.Id);

        builder.Property(todo => todo.Id)
            .ValueGeneratedNever();

        builder.Property(todo => todo.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(todo => todo.IsCompleted)
            .IsRequired();
    }
}
```

Database configuration belongs in Infrastructure, not Domain.
