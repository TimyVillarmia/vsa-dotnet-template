using ErrorOr;
using MediatR;
using VsaTemplate.Application.Abstractions.Data;
using VsaTemplate.Domain.Todos;

namespace VsaTemplate.Application.Features.Todos.CreateTodo;

public sealed class CreateTodoHandler
    : IRequestHandler<CreateTodoCommand, ErrorOr<CreateTodoResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateTodoHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<CreateTodoResponse>> Handle(
        CreateTodoCommand request,
        CancellationToken cancellationToken)
    {
        var todoResult = Todo.Create(request.Title);

        if (todoResult.IsError)
        {
            return todoResult.Errors;
        }

        var todo = todoResult.Value;

        _dbContext.Todos.Add(todo);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateTodoResponse(
            todo.Id,
            todo.Title,
            todo.IsCompleted);
    }
}
