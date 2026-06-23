using MediatR;
using Project.API.Common;
using Project.Application.Features.Todos.CreateTodo;
using Project.Application.Features.Todos.GetTodoById;

namespace Project.API.Endpoints.Todos;

public sealed class TodoEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/todos")
            .WithTags("Todos");

        group.MapPost("/", CreateTodo)
            .WithName(nameof(CreateTodo))
            .Produces<CreateTodoResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet("/{id:guid}", GetTodoById)
            .WithName(nameof(GetTodoById))
            .Produces<GetTodoByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> CreateTodo(
        CreateTodoRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new CreateTodoCommand(request.Title);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            todo => Results.Created($"/api/todos/{todo.Id}", todo),
            errors => errors.ToProblem());
    }

    private static async Task<IResult> GetTodoById(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetTodoByIdQuery(id);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(
            todo => Results.Ok(todo),
            errors => errors.ToProblem());
    }
}