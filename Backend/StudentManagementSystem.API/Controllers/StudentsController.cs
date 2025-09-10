using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StudentManagementSystem.Application.Commands.Students;
using StudentManagementSystem.Application.Queries.Students;
using StudentManagementSystem.Application.DTOs;
using AutoMapper;
using StudentManagementSystem.API.Models;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
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

    /// <summary>
    /// Gets all students with optional pagination and filtering
    /// </summary>
    /// <param name="pageNumber">Page number for pagination</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="department">Filter by department</param>
    /// <param name="grade">Filter by grade</param>
    /// <returns>List of students</returns>
    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentDto>>), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<ActionResult<ApiResponse<IEnumerable<StudentDto>>>> GetAllStudents(
        [FromQuery] int? pageNumber = null,
        [FromQuery] int? pageSize = null,
        [FromQuery] string? department = null,
        [FromQuery] int? grade = null)
    {
        var query = new GetAllStudentsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            Department = department,
            Grade = grade
        };

        var students = await _mediator.Send(query);
        var studentDtos = _mapper.Map<IEnumerable<StudentDto>>(students);
        return Ok(ApiResponse<IEnumerable<StudentDto>>.SuccessResponse(studentDtos, "Students retrieved successfully"));
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

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var command = new DeleteStudentCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
