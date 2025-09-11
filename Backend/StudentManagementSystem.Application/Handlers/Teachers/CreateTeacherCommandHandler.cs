using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Commands.Teachers;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Handlers.Teachers;

public class CreateTeacherCommandHandler : IRequestHandler<CreateTeacherCommand, TeacherDto>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IMapper _mapper;

    public CreateTeacherCommandHandler(ITeacherRepository teacherRepository, IMapper mapper)
    {
        _teacherRepository = teacherRepository;
        _mapper = mapper;
    }

    public async Task<TeacherDto> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
    {
        // Check if email already exists
        if (await _teacherRepository.EmailExistsAsync(request.Email))
        {
            throw new InvalidOperationException("A teacher with this email already exists.");
        }

        // Check if employee ID already exists
        if (await _teacherRepository.EmployeeIdExistsAsync(request.EmployeeId))
        {
            throw new InvalidOperationException("A teacher with this employee ID already exists.");
        }

        var teacher = new Teacher
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            EmployeeId = request.EmployeeId,
            PhoneNumber = request.PhoneNumber,
            Department = request.Department,
            Specialty = request.Specialty,
            HireDate = request.HireDate,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var createdTeacher = await _teacherRepository.AddAsync(teacher);
        return _mapper.Map<TeacherDto>(createdTeacher);
    }
}