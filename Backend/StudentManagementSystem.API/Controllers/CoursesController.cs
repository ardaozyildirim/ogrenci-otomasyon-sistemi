using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StudentManagementSystem.Application.Commands.Courses;
using StudentManagementSystem.Application.Queries.Courses;
using StudentManagementSystem.Application.DTOs;
using AutoMapper;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[EnableRateLimiting("ApiPolicy")]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CoursesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses(
        [FromQuery] int? pageNumber = null,
        [FromQuery] int? pageSize = null,
        [FromQuery] int? teacherId = null,
        [FromQuery] string? department = null)
    {
        var query = new GetAllCoursesQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TeacherId = teacherId,
            Department = department
        };

        var courses = await _mediator.Send(query);
        return Ok(_mapper.Map<IEnumerable<CourseDto>>(courses));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDto>> GetCourse(int id)
    {
        var query = new GetCourseByIdQuery { Id = id };
        var course = await _mediator.Send(query);

        if (course == null)
            return NotFound();

        return Ok(_mapper.Map<CourseDto>(course));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<int>> CreateCourse(CreateCourseCommand command)
    {
        var courseId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCourse), new { id = courseId }, courseId);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> UpdateCourse(int id, UpdateCourseCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var command = new DeleteCourseCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}