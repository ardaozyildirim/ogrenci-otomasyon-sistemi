using FluentValidation;
using StudentManagementSystem.Application.Commands.Students;

namespace StudentManagementSystem.Application.Validators;

public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0");

        RuleFor(x => x.StudentNumber)
            .NotEmpty()
            .WithMessage("Student number is required")
            .Matches(@"^[0-9]{4}[A-Z]{2}[0-9]{3}$")
            .WithMessage("Student number must be in format: YYYYAAXXX (e.g., 2024CS001)");

        RuleFor(x => x.Department)
            .MaximumLength(100)
            .WithMessage("Department cannot exceed 100 characters");

        RuleFor(x => x.Grade)
            .InclusiveBetween(1, 4)
            .WithMessage("Grade must be between 1 and 4")
            .When(x => x.Grade.HasValue);

        RuleFor(x => x.ClassName)
            .MaximumLength(50)
            .WithMessage("Class name cannot exceed 50 characters");
    }
}
