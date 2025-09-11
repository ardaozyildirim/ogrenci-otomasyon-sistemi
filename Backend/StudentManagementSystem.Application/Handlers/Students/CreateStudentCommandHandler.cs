using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Commands.Students;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Handlers.Students;

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, StudentDto>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public CreateStudentCommandHandler(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    public async Task<StudentDto> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        // Check if email already exists
        if (await _studentRepository.EmailExistsAsync(request.Email))
        {
            throw new InvalidOperationException("A student with this email already exists.");
        }

        // Check if student number already exists
        if (await _studentRepository.StudentNumberExistsAsync(request.StudentNumber))
        {
            throw new InvalidOperationException("A student with this student number already exists.");
        }

        // Create new student
        var student = _mapper.Map<Student>(request);
        student.CreatedAt = DateTime.UtcNow;
        student.IsActive = true;

        var createdStudent = await _studentRepository.AddAsync(student);
        return _mapper.Map<StudentDto>(createdStudent);
    }
}