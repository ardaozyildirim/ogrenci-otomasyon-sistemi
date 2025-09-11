using FluentValidation;
using StudentManagementSystem.Application.Commands.Courses;

namespace StudentManagementSystem.Application.Validators.Courses;

public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Course ID must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Course name is required.")
            .MaximumLength(200).WithMessage("Course name cannot exceed 200 characters.");

        RuleFor(x => x.CourseCode)
            .NotEmpty().WithMessage("Course code is required.")
            .MaximumLength(20).WithMessage("Course code cannot exceed 20 characters.");

        RuleFor(x => x.Credits)
            .GreaterThan(0).WithMessage("Credits must be greater than 0.")
            .LessThanOrEqualTo(10).WithMessage("Credits cannot exceed 10.");

        RuleFor(x => x.TeacherId)
            .GreaterThan(0).WithMessage("Teacher ID must be greater than 0.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Capacity must be greater than 0.")
            .LessThanOrEqualTo(500).WithMessage("Capacity cannot exceed 500.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid course status.");
    }
}