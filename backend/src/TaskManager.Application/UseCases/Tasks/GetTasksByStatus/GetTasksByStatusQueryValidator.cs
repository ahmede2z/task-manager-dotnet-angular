using FluentValidation;

namespace TaskManager.Application.UseCases.Tasks.GetTasksByStatus;

public sealed class GetTasksByStatusQueryValidator : AbstractValidator<GetTasksByStatusQuery>
{
    public GetTasksByStatusQueryValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum();
    }
}
