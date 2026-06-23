using Project.Domain.Todos;

namespace Project.UnitTests.Domain.Todos;

public sealed class TodoTests
{
    [Fact]
    public void Create_ShouldReturnTodo_WhenTitleIsValid()
    {
        var result = Todo.Create("Learn VSA template");

        Assert.False(result.IsError);
        Assert.Equal("Learn VSA template", result.Value.Title);
        Assert.False(result.Value.IsCompleted);
    }

    [Fact]
    public void Create_ShouldReturnError_WhenTitleIsEmpty()
    {
        var result = Todo.Create("");

        Assert.True(result.IsError);
        Assert.Contains(result.Errors, error => error.Code == "Todo.TitleRequired");
    }

    [Fact]
    public void Complete_ShouldMarkTodoAsCompleted()
    {
        var result = Todo.Create("Learn testing");

        result.Value.Complete();

        Assert.True(result.Value.IsCompleted);
    }
}