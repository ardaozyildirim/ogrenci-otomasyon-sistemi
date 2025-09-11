using FluentValidation;
using StudentManagementSystem.Application.Queries.Teachers;

namespace StudentManagementSystem.Application.Validators.Teachers;

public class GetTeacherByEmployeeIdQueryValidator : AbstractValidator<GetTeacherByEmployeeIdQuery>
{
    public GetTeacherByEmployeeIdQueryValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.")
            .MaximumLength(20).WithMessage("Employee ID cannot exceed 20 characters.");
    }
}