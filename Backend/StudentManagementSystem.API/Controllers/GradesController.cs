using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.Commands.Grades;
using StudentManagementSystem.Application.Queries.Grades;
using StudentManagementSystem.Application.DTOs;
using AutoMapper;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GradesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public GradesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<IEnumerable<GradeDto>>> GetStudentGrades(int studentId)
    {
        var query = new GetStudentGradesQuery { StudentId = studentId };
        var grades = await _mediator.Send(query);

        return Ok(_mapper.Map<IEnumerable<GradeDto>>(grades));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<int>> AssignGrade(AssignGradeCommand command)
    {
        var gradeId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetStudentGrades), new { studentId = command.StudentId }, gradeId);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> UpdateGrade(int id, UpdateGradeCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        await _mediator.Send(command);
        return NoContent();
    }
}
