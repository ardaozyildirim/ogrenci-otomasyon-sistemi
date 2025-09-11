using FluentValidation;
using StudentManagementSystem.Application.Commands.Students;

namespace StudentManagementSystem.Application.Validators.Students;

public class DeleteStudentCommandValidator : AbstractValidator<DeleteStudentCommand>
{
    public DeleteStudentCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Student ID must be greater than 0.");
    }
}