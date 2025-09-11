using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Commands.Students;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Handlers.Students;

public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, StudentDto>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public UpdateStudentCommandHandler(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    public async Task<StudentDto> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        // Find existing student
        var existingStudent = await _studentRepository.GetByIdAsync(request.Id);
        if (existingStudent == null)
        {
            throw new KeyNotFoundException($"Student with ID {request.Id} not found.");
        }

        // Check if email is being changed and if it already exists
        if (existingStudent.Email != request.Email && await _studentRepository.EmailExistsAsync(request.Email))
        {
            throw new InvalidOperationException("A student with this email already exists.");
        }

        // Check if student number is being changed and if it already exists
        if (existingStudent.StudentNumber != request.StudentNumber && await _studentRepository.StudentNumberExistsAsync(request.StudentNumber))
        {
            throw new InvalidOperationException("A student with this student number already exists.");
        }

        // Update student properties
        existingStudent.FirstName = request.FirstName;
        existingStudent.LastName = request.LastName;
        existingStudent.Email = request.Email;
        existingStudent.StudentNumber = request.StudentNumber;
        existingStudent.DateOfBirth = request.DateOfBirth;
        existingStudent.PhoneNumber = request.PhoneNumber;
        existingStudent.Address = request.Address;
        existingStudent.EnrollmentDate = request.EnrollmentDate;
        existingStudent.IsActive = request.IsActive;
        existingStudent.UpdatedAt = DateTime.UtcNow;

        var updatedStudent = await _studentRepository.UpdateAsync(existingStudent);
        return _mapper.Map<StudentDto>(updatedStudent);
    }
}