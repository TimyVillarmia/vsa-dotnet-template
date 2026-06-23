using System.Net;
using System.Net.Http.Json;
using VsaTemplate.IntegrationTests.Infrastructure;

namespace VsaTemplate.IntegrationTests.Todos;

public sealed class TodoEndpointTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public TodoEndpointTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateTodo_ShouldReturnCreated_WhenRequestIsValid()
    {
        var response = await _client.PostAsJsonAsync("/api/todos", new
        {
            title = "Learn integration testing"
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<CreateTodoResponse>();

        Assert.NotNull(body);
        Assert.NotEqual(Guid.Empty, body.Id);
        Assert.Equal("Learn integration testing", body.Title);
        Assert.False(body.IsCompleted);
    }

    [Fact]
    public async Task GetTodoById_ShouldReturnTodo_WhenTodoExists()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/todos", new
        {
            title = "Test get todo"
        });

        var createdTodo = await createResponse.Content
            .ReadFromJsonAsync<CreateTodoResponse>();

        Assert.NotNull(createdTodo);

        var getResponse = await _client.GetAsync($"/api/todos/{createdTodo.Id}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var todo = await getResponse.Content.ReadFromJsonAsync<GetTodoByIdResponse>();

        Assert.NotNull(todo);
        Assert.Equal(createdTodo.Id, todo.Id);
        Assert.Equal("Test get todo", todo.Title);
        Assert.False(todo.IsCompleted);
    }

    [Fact]
    public async Task GetTodoById_ShouldReturnNotFound_WhenTodoDoesNotExist()
    {
        var id = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/todos/{id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private sealed record CreateTodoResponse(
        Guid Id,
        string Title,
        bool IsCompleted);

    private sealed record GetTodoByIdResponse(
        Guid Id,
        string Title,
        bool IsCompleted);
}