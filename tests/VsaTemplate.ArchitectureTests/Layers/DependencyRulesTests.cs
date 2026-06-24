using NetArchTest.Rules;
using VsaTemplate.Domain.Common;
using ApplicationDependencyInjection = VsaTemplate.Application.DependencyInjection;
using InfrastructureDependencyInjection = VsaTemplate.Infrastructure.DependencyInjection;

namespace VsaTemplate.ArchitectureTests.Layers;

public sealed class DependencyRulesTests
{
    [Fact]
    public void Domain_ShouldNotDependOn_Application()
    {
        var result = Types
            .InAssembly(typeof(Entity).Assembly)
            .ShouldNot()
            .HaveDependencyOn("VsaTemplate.Application")
            .GetResult();

        Assert.True(result.IsSuccessful, GetFailingTypes(result));
    }

    [Fact]
    public void Domain_ShouldNotDependOn_Infrastructure()
    {
        var result = Types
            .InAssembly(typeof(Entity).Assembly)
            .ShouldNot()
            .HaveDependencyOn("VsaTemplate.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful, GetFailingTypes(result));
    }

    [Fact]
    public void Domain_ShouldNotDependOn_Api()
    {
        var result = Types
            .InAssembly(typeof(Entity).Assembly)
            .ShouldNot()
            .HaveDependencyOn("VsaTemplate.API")
            .GetResult();

        Assert.True(result.IsSuccessful, GetFailingTypes(result));
    }

    [Fact]
    public void Application_ShouldNotDependOn_Infrastructure()
    {
        var result = Types
            .InAssembly(typeof(ApplicationDependencyInjection).Assembly)
            .ShouldNot()
            .HaveDependencyOn("VsaTemplate.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful, GetFailingTypes(result));
    }

    [Fact]
    public void Application_ShouldNotDependOn_Api()
    {
        var result = Types
            .InAssembly(typeof(ApplicationDependencyInjection).Assembly)
            .ShouldNot()
            .HaveDependencyOn("VsaTemplate.API")
            .GetResult();

        Assert.True(result.IsSuccessful, GetFailingTypes(result));
    }

    [Fact]
    public void Infrastructure_ShouldNotDependOn_Api()
    {
        var result = Types
            .InAssembly(typeof(InfrastructureDependencyInjection).Assembly)
            .ShouldNot()
            .HaveDependencyOn("VsaTemplate.API")
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
