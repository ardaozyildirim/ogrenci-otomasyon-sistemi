using FluentValidation;
using StudentManagementSystem.Application.Queries.Teachers;

namespace StudentManagementSystem.Application.Validators.Teachers;

public class GetTeacherByEmailQueryValidator : AbstractValidator<GetTeacherByEmailQuery>
{
    public GetTeacherByEmailQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(200).WithMessage("Email cannot exceed 200 characters.");
    }
}