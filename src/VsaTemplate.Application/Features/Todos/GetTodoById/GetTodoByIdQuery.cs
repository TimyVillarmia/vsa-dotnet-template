using ErrorOr;
using MediatR;

namespace VsaTemplate.Application.Features.Todos.GetTodoById;

public sealed record GetTodoByIdQuery(Guid Id)
    : IRequest<ErrorOr<GetTodoByIdResponse>>;
