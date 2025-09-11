using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Teachers;

namespace StudentManagementSystem.Application.Handlers.Teachers;

public class GetTeacherByEmailQueryHandler : IRequestHandler<GetTeacherByEmailQuery, TeacherDto?>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IMapper _mapper;

    public GetTeacherByEmailQueryHandler(ITeacherRepository teacherRepository, IMapper mapper)
    {
        _teacherRepository = teacherRepository;
        _mapper = mapper;
    }

    public async Task<TeacherDto?> Handle(GetTeacherByEmailQuery request, CancellationToken cancellationToken)
    {
        var teacher = await _teacherRepository.GetByEmailAsync(request.Email);
        return teacher != null ? _mapper.Map<TeacherDto>(teacher) : null;
    }
}