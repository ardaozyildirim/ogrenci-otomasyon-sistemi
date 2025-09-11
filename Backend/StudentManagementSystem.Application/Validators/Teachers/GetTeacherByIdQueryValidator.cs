using FluentValidation;
using StudentManagementSystem.Application.Queries.Teachers;

namespace StudentManagementSystem.Application.Validators.Teachers;

public class GetTeacherByIdQueryValidator : AbstractValidator<GetTeacherByIdQuery>
{
    public GetTeacherByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Teacher ID must be greater than 0.");
    }
}