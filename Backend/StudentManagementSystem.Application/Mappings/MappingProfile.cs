using AutoMapper;
using StudentManagementSystem.Application.Commands.Students;
using StudentManagementSystem.Application.Commands.Grades;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entity to DTO mappings
        CreateMap<User, UserDto>();
        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
        CreateMap<Teacher, TeacherDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.User.FullName));
        CreateMap<Grade, GradeDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.User.FullName))
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name));

        // Command to Entity mappings
        CreateMap<CreateStudentCommand, Student>();
        CreateMap<UpdateStudentCommand, Student>();
        CreateMap<AssignGradeCommand, Grade>();
        CreateMap<UpdateGradeCommand, Grade>();
    }
}
