using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Students;

namespace StudentManagementSystem.Application.Handlers.Students;

public class GetStudentByEmailQueryHandler : IRequestHandler<GetStudentByEmailQuery, StudentDto?>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public GetStudentByEmailQueryHandler(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    public async Task<StudentDto?> Handle(GetStudentByEmailQuery request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByEmailAsync(request.Email);
        return student != null ? _mapper.Map<StudentDto>(student) : null;
    }
}