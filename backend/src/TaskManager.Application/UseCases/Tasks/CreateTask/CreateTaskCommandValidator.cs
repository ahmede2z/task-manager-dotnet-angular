using FluentValidation;
using TaskManager.Domain.Common;

namespace TaskManager.Application.UseCases.Tasks.CreateTask;

public sealed class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(FieldLengths.TaskTitle);

        RuleFor(x => x.Description)
            .MaximumLength(FieldLengths.Description);

        RuleFor(x => x.Status)
            .IsInEnum();

        RuleFor(x => x.ProjectId)
            .GreaterThan(0);
    }
}
