using FluentValidation;
using StudentManagementSystem.Application.Commands.Grades;

namespace StudentManagementSystem.Application.Validators;

public class AssignGradeCommandValidator : AbstractValidator<AssignGradeCommand>
{
    public AssignGradeCommandValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0)
            .WithMessage("Student ID must be greater than 0");

        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .WithMessage("Course ID must be greater than 0");

        RuleFor(x => x.Score)
            .InclusiveBetween(0, 100)
            .WithMessage("Score must be between 0 and 100");

        RuleFor(x => x.GradeType)
            .MaximumLength(50)
            .WithMessage("Grade type cannot exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.GradeType));

        RuleFor(x => x.Comment)
            .MaximumLength(500)
            .WithMessage("Comment cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Comment));
    }
}
