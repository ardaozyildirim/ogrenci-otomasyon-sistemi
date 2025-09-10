using FluentValidation;
using StudentManagementSystem.Application.Commands.Students;

namespace StudentManagementSystem.Application.Validators;

public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be greater than 0");

        RuleFor(x => x.StudentNumber)
            .NotEmpty().WithMessage("Student number is required")
            .Matches(@"^\d{4}[A-Z]{2}\d{3}$").WithMessage("Student number must be in format YYYYAAXXX (e.g., 2024IT001)")
            .MaximumLength(9).WithMessage("Student number cannot exceed 9 characters");

        RuleFor(x => x.Department)
            .MaximumLength(100).WithMessage("Department cannot exceed 100 characters")
            .Matches(@"^[a-zA-Z\s\-&]+$").When(x => !string.IsNullOrEmpty(x.Department))
            .WithMessage("Department can only contain letters, spaces, hyphens, and ampersands");

        RuleFor(x => x.Grade)
            .InclusiveBetween(1, 12).When(x => x.Grade.HasValue)
            .WithMessage("Grade must be between 1 and 12");

        RuleFor(x => x.ClassName)
            .MaximumLength(50).WithMessage("Class name cannot exceed 50 characters")
            .Matches(@"^[a-zA-Z0-9\s\-]+$").When(x => !string.IsNullOrEmpty(x.ClassName))
            .WithMessage("Class name can only contain letters, numbers, spaces, and hyphens");
    }
}