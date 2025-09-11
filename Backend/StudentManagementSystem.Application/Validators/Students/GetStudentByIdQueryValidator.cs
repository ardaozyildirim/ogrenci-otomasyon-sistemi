using FluentValidation;
using StudentManagementSystem.Application.Queries.Students;

namespace StudentManagementSystem.Application.Validators.Students;

public class GetStudentByIdQueryValidator : AbstractValidator<GetStudentByIdQuery>
{
    public GetStudentByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Student ID must be greater than 0.");
    }
}