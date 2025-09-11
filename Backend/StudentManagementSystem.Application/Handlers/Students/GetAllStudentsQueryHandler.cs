using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Students;

namespace StudentManagementSystem.Application.Handlers.Students;

public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, IEnumerable<StudentDto>>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public GetAllStudentsQueryHandler(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StudentDto>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        var students = await _studentRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<StudentDto>>(students);
    }
}