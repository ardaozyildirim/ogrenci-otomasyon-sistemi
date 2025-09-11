using FluentValidation;
using StudentManagementSystem.Application.Commands.Grades;

namespace StudentManagementSystem.Application.Validators.Grades;

public class UpdateGradeCommandValidator : AbstractValidator<UpdateGradeCommand>
{
    public UpdateGradeCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Grade ID must be greater than 0.");

        RuleFor(x => x.Score)
            .GreaterThanOrEqualTo(0).WithMessage("Score must be greater than or equal to 0.")
            .LessThanOrEqualTo(100).WithMessage("Score cannot exceed 100.");

        RuleFor(x => x.LetterGrade)
            .MaximumLength(5).WithMessage("Letter grade cannot exceed 5 characters.")
            .When(x => !string.IsNullOrEmpty(x.LetterGrade));

        RuleFor(x => x.Comments)
            .MaximumLength(500).WithMessage("Comments cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Comments));

        RuleFor(x => x.GradeType)
            .NotEmpty().WithMessage("Grade type is required.")
            .MaximumLength(20).WithMessage("Grade type cannot exceed 20 characters.");

        RuleFor(x => x.GradeDate)
            .LessThanOrEqualTo(DateTime.Now.AddDays(1)).WithMessage("Grade date cannot be in the future.");
    }
}