using FluentValidation;
using StudentManagementSystem.Application.Queries.Students;

namespace StudentManagementSystem.Application.Validators.Students;

public class GetStudentByEmailQueryValidator : AbstractValidator<GetStudentByEmailQuery>
{
    public GetStudentByEmailQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(200).WithMessage("Email cannot exceed 200 characters.");
    }
}