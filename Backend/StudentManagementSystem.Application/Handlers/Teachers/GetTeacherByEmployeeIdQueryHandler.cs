using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Teachers;

namespace StudentManagementSystem.Application.Handlers.Teachers;

public class GetTeacherByEmployeeIdQueryHandler : IRequestHandler<GetTeacherByEmployeeIdQuery, TeacherDto?>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IMapper _mapper;

    public GetTeacherByEmployeeIdQueryHandler(ITeacherRepository teacherRepository, IMapper mapper)
    {
        _teacherRepository = teacherRepository;
        _mapper = mapper;
    }

    public async Task<TeacherDto?> Handle(GetTeacherByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
        var teacher = await _teacherRepository.GetByEmployeeIdAsync(request.EmployeeId);
        return teacher != null ? _mapper.Map<TeacherDto>(teacher) : null;
    }
}