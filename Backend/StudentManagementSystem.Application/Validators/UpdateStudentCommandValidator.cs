using FluentValidation;
using StudentManagementSystem.Application.Commands.Students;

namespace StudentManagementSystem.Application.Validators;

public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Student ID must be greater than 0");


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