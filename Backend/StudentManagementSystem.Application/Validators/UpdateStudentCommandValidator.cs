using FluentValidation;
using StudentManagementSystem.Application.Commands.Students;

namespace StudentManagementSystem.Application.Validators;

public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Student ID must be greater than 0");

        RuleFor(x => x.Department)
            .MaximumLength(100)
            .WithMessage("Department cannot exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Department));

        RuleFor(x => x.Grade)
            .InclusiveBetween(1, 4)
            .WithMessage("Grade must be between 1 and 4")
            .When(x => x.Grade.HasValue);

        RuleFor(x => x.ClassName)
            .MaximumLength(50)
            .WithMessage("Class name cannot exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.ClassName));
    }
}
