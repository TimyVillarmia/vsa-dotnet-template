using ApplicationDependencyInjection = Project.Application.DependencyInjection;
using InfrastructureDependencyInjection = Project.Infrastructure.DependencyInjection;
using NetArchTest.Rules;
using Project.Domain.Common;

namespace Project.ArchitectureTests.Layers;

public sealed class DependencyRulesTests
{
    [Fact]
    public void Domain_ShouldNotDependOn_Application()
    {
        var result = Types
            .InAssembly(typeof(Entity).Assembly)
            .ShouldNot()
            .HaveDependencyOn("Project.Application")
            .GetResult();

        Assert.True(result.IsSuccessful, GetFailingTypes(result));
    }

    [Fact]
    public void Domain_ShouldNotDependOn_Infrastructure()
    {
        var result = Types
            .InAssembly(typeof(Entity).Assembly)
            .ShouldNot()
            .HaveDependencyOn("Project.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful, GetFailingTypes(result));
    }

    [Fact]
    public void Domain_ShouldNotDependOn_Api()
    {
        var result = Types
            .InAssembly(typeof(Entity).Assembly)
            .ShouldNot()
            .HaveDependencyOn("Project.API")
            .GetResult();

        Assert.True(result.IsSuccessful, GetFailingTypes(result));
    }

    [Fact]
    public void Application_ShouldNotDependOn_Infrastructure()
    {
        var result = Types
            .InAssembly(typeof(ApplicationDependencyInjection).Assembly)
            .ShouldNot()
            .HaveDependencyOn("Project.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful, GetFailingTypes(result));
    }

    [Fact]
    public void Application_ShouldNotDependOn_Api()
    {
        var result = Types
            .InAssembly(typeof(ApplicationDependencyInjection).Assembly)
            .ShouldNot()
            .HaveDependencyOn("Project.API")
            .GetResult();

        Assert.True(result.IsSuccessful, GetFailingTypes(result));
    }

    [Fact]
    public void Infrastructure_ShouldNotDependOn_Api()
    {
        var result = Types
            .InAssembly(typeof(InfrastructureDependencyInjection).Assembly)
            .ShouldNot()
            .HaveDependencyOn("Project.API")
            .GetResult();

        Assert.True(result.IsSuccessful, GetFailingTypes(result));
    }

    private static string GetFailingTypes(TestResult result)
    {
        if (result.FailingTypes is null)
        {
            return string.Empty;
        }

        return string.Join(
            Environment.NewLine,
            result.FailingTypes.Select(type => type.FullName));
    }
}