using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StudentManagementSystem.Application.Commands.Teachers;
using StudentManagementSystem.Application.Queries.Teachers;
using StudentManagementSystem.Application.DTOs;
using AutoMapper;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[EnableRateLimiting("ApiPolicy")]
public class TeachersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public TeachersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<TeacherDto>>> GetAllTeachers(
        [FromQuery] int? pageNumber = null,
        [FromQuery] int? pageSize = null,
        [FromQuery] string? department = null)
    {
        var query = new GetAllTeachersQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            Department = department
        };

        var teachers = await _mediator.Send(query);
        return Ok(_mapper.Map<IEnumerable<TeacherDto>>(teachers));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeacherDto>> GetTeacher(int id)
    {
        var query = new GetTeacherByIdQuery { Id = id };
        var teacher = await _mediator.Send(query);

        if (teacher == null)
            return NotFound();

        return Ok(_mapper.Map<TeacherDto>(teacher));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<int>> CreateTeacher(CreateTeacherCommand command)
    {
        var teacherId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTeacher), new { id = teacherId }, teacherId);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateTeacher(int id, UpdateTeacherCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTeacher(int id)
    {
        var command = new DeleteTeacherCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
