using ErrorOr;
using MediatR;

namespace VsaTemplate.Application.Features.Todos.CreateTodo;

public sealed record CreateTodoCommand(string Title)
    : IRequest<ErrorOr<CreateTodoResponse>>;
