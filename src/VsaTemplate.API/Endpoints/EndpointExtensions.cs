using System.Reflection;

namespace VsaTemplate.API.Endpoints;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        var endpointTypes = Assembly.GetExecutingAssembly()
            .DefinedTypes
            .Where(type =>
                !type.IsAbstract &&
                !type.IsInterface &&
                typeof(IEndpoint).IsAssignableFrom(type));

        foreach (var endpointType in endpointTypes)
        {
            services.AddTransient(typeof(IEndpoint), endpointType);
        }

        return services;
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }

        return app;
    }
}
