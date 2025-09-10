using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StudentManagementSystem.Application.Commands.Students;
using StudentManagementSystem.Application.Queries.Students;
using StudentManagementSystem.Application.DTOs;
using AutoMapper;
using StudentManagementSystem.API.Attributes;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[EnableRateLimiting("ApiPolicy")]
public class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public StudentsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetStudent(int id)
    {
        var query = new GetStudentByIdQuery { Id = id };
        var student = await _mediator.Send(query);

        if (student == null)
            return NotFound();

        return Ok(_mapper.Map<StudentDto>(student));
    }

    [HttpGet("course/{courseId}")]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudentsByCourse(int courseId)
    {
        var query = new GetStudentsByCourseQuery { CourseId = courseId };
        var students = await _mediator.Send(query);

        return Ok(_mapper.Map<IEnumerable<StudentDto>>(students));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<int>> CreateStudent(CreateStudentCommand command)
    {
        var studentId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetStudent), new { id = studentId }, studentId);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> UpdateStudent(int id, UpdateStudentCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{studentId}/enroll/{courseId}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> EnrollStudentInCourse(int studentId, int courseId)
    {
        var command = new EnrollStudentInCourseCommand
        {
            StudentId = studentId,
            CourseId = courseId
        };

        await _mediator.Send(command);
        return Ok();
    }
}
