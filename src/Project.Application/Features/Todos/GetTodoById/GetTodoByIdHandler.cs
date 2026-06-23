using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Project.Application.Abstractions.Data;
using Project.Domain.Todos;

namespace Project.Application.Features.Todos.GetTodoById;

public sealed class GetTodoByIdHandler
    : IRequestHandler<GetTodoByIdQuery, ErrorOr<GetTodoByIdResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetTodoByIdHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<GetTodoByIdResponse>> Handle(
        GetTodoByIdQuery request,
        CancellationToken cancellationToken)
    {
        var todo = await _dbContext.Todos
            .AsNoTracking()
            .Where(todo => todo.Id == request.Id)
            .Select(todo => new GetTodoByIdResponse(
                todo.Id,
                todo.Title,
                todo.IsCompleted))
            .FirstOrDefaultAsync(cancellationToken);

        if (todo is null)
        {
            return TodoErrors.NotFound(request.Id);
        }

        return todo;
    }
}