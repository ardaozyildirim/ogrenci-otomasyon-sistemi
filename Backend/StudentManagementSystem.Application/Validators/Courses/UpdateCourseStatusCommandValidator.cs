using FluentValidation;
using StudentManagementSystem.Application.Commands.Courses;

namespace StudentManagementSystem.Application.Validators.Courses;

public class UpdateCourseStatusCommandValidator : AbstractValidator<UpdateCourseStatusCommand>
{
    public UpdateCourseStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Course ID must be greater than 0.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid course status.");
    }
}