using ErrorOr;
using MediatR;

namespace Project.Application.Features.Todos.CreateTodo;

public sealed record CreateTodoCommand(string Title)
    : IRequest<ErrorOr<CreateTodoResponse>>;