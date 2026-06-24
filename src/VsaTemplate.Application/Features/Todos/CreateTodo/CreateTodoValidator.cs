using FluentValidation;

namespace VsaTemplate.Application.Features.Todos.CreateTodo;

public sealed class CreateTodoValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoValidator()
    {
        RuleFor(command => command.Title)
            .NotEmpty()
            .MaximumLength(200);
    }
}
