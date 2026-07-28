using FluentValidation;
using TaskManager.Domain.Common;

namespace TaskManager.Application.UseCases.Projects.CreateProject;

public sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(FieldLengths.ProjectName);

        RuleFor(x => x.Description)
            .MaximumLength(FieldLengths.Description);
    }
}
