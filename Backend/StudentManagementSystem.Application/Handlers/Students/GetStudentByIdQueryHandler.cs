using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Students;

namespace StudentManagementSystem.Application.Handlers.Students;

public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentDto?>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public GetStudentByIdQueryHandler(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    public async Task<StudentDto?> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.Id);
        return student != null ? _mapper.Map<StudentDto>(student) : null;
    }
}