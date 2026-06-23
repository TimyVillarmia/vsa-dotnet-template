using ErrorOr;
using MediatR;

namespace Project.Application.Features.Todos.GetTodoById;

public sealed record GetTodoByIdQuery(Guid Id)
    : IRequest<ErrorOr<GetTodoByIdResponse>>;