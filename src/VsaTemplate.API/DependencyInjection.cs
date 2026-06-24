using VsaTemplate.API.Endpoints;

namespace VsaTemplate.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddProblemDetails();
        services.AddEndpoints();

        services.AddHealthChecks();


        return services;
    }
}
