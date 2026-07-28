using FluentValidation;
using TaskManager.Domain.Common;

namespace TaskManager.Application.UseCases.Projects.UpdateProject;

public sealed class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(FieldLengths.ProjectName);

        RuleFor(x => x.Description)
            .MaximumLength(FieldLengths.Description);
    }
}
