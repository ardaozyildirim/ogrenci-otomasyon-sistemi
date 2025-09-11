using FluentValidation;
using StudentManagementSystem.Application.Commands.Teachers;

namespace StudentManagementSystem.Application.Validators.Teachers;

public class DeleteTeacherCommandValidator : AbstractValidator<DeleteTeacherCommand>
{
    public DeleteTeacherCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Teacher ID must be greater than 0.");
    }
}