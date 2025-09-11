using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Commands.Teachers;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Handlers.Teachers;

public class UpdateTeacherCommandHandler : IRequestHandler<UpdateTeacherCommand, TeacherDto>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IMapper _mapper;

    public UpdateTeacherCommandHandler(ITeacherRepository teacherRepository, IMapper mapper)
    {
        _teacherRepository = teacherRepository;
        _mapper = mapper;
    }

    public async Task<TeacherDto> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
    {
        var existingTeacher = await _teacherRepository.GetByIdAsync(request.Id);
        if (existingTeacher == null)
        {
            throw new InvalidOperationException("Teacher not found.");
        }

        // Check if email is being changed and if the new email already exists
        if (request.Email != existingTeacher.Email && await _teacherRepository.EmailExistsAsync(request.Email))
        {
            throw new InvalidOperationException("A teacher with this email already exists.");
        }

        // Check if employee ID is being changed and if the new employee ID already exists
        if (request.EmployeeId != existingTeacher.EmployeeId && await _teacherRepository.EmployeeIdExistsAsync(request.EmployeeId))
        {
            throw new InvalidOperationException("A teacher with this employee ID already exists.");
        }

        // Update teacher properties
        existingTeacher.FirstName = request.FirstName;
        existingTeacher.LastName = request.LastName;
        existingTeacher.Email = request.Email;
        existingTeacher.EmployeeId = request.EmployeeId;
        existingTeacher.PhoneNumber = request.PhoneNumber;
        existingTeacher.Department = request.Department;
        existingTeacher.Specialty = request.Specialty;
        existingTeacher.HireDate = request.HireDate;
        existingTeacher.IsActive = request.IsActive;
        existingTeacher.UpdatedAt = DateTime.UtcNow;

        var updatedTeacher = await _teacherRepository.UpdateAsync(existingTeacher);
        return _mapper.Map<TeacherDto>(updatedTeacher);
    }
}