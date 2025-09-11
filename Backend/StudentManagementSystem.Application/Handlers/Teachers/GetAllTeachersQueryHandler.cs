using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Teachers;

namespace StudentManagementSystem.Application.Handlers.Teachers;

public class GetAllTeachersQueryHandler : IRequestHandler<GetAllTeachersQuery, IEnumerable<TeacherDto>>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IMapper _mapper;

    public GetAllTeachersQueryHandler(ITeacherRepository teacherRepository, IMapper mapper)
    {
        _teacherRepository = teacherRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TeacherDto>> Handle(GetAllTeachersQuery request, CancellationToken cancellationToken)
    {
        var teachers = await _teacherRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<TeacherDto>>(teachers);
    }
}