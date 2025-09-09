using AutoMapper;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<Student, StudentDto>();
        CreateMap<Teacher, TeacherDto>();
        CreateMap<Course, CourseDto>();
    }
}
