using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.Commands.Teachers;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Teachers;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class TeachersController : ControllerBase
{
    private readonly IMediator _mediator;

    public TeachersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all teachers - Only Admin can access
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeacherDto>>> GetAllTeachers()
    {
        try
        {
            var query = new GetAllTeachersQuery();
            var teachers = await _mediator.Send(query);
            return Ok(teachers);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get teacher by ID - Only Admin can access
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TeacherDto>> GetTeacherById(int id)
    {
        try
        {
            var query = new GetTeacherByIdQuery(id);
            var teacher = await _mediator.Send(query);
            
            if (teacher == null)
            {
                return NotFound(new { message = "Teacher not found." });
            }

            return Ok(teacher);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create a new teacher - Only Admin can access
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TeacherDto>> CreateTeacher([FromBody] CreateTeacherDto createTeacherDto)
    {
        try
        {
            var command = new CreateTeacherCommand
            {
                FirstName = createTeacherDto.FirstName,
                LastName = createTeacherDto.LastName,
                Email = createTeacherDto.Email,
                EmployeeId = createTeacherDto.EmployeeId,
                PhoneNumber = createTeacherDto.PhoneNumber,
                Department = createTeacherDto.Department,
                Specialty = createTeacherDto.Specialty,
                HireDate = createTeacherDto.HireDate
            };

            var teacher = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetTeacherById), new { id = teacher.Id }, teacher);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update a teacher - Only Admin can access
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<TeacherDto>> UpdateTeacher(int id, [FromBody] UpdateTeacherDto updateTeacherDto)
    {
        try
        {
            var command = new UpdateTeacherCommand
            {
                Id = id,
                FirstName = updateTeacherDto.FirstName,
                LastName = updateTeacherDto.LastName,
                Email = updateTeacherDto.Email,
                EmployeeId = updateTeacherDto.EmployeeId,
                PhoneNumber = updateTeacherDto.PhoneNumber,
                Department = updateTeacherDto.Department,
                Specialty = updateTeacherDto.Specialty,
                HireDate = updateTeacherDto.HireDate,
                IsActive = updateTeacherDto.IsActive
            };

            var teacher = await _mediator.Send(command);
            return Ok(teacher);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete a teacher - Only Admin can access
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTeacher(int id)
    {
        try
        {
            var command = new DeleteTeacherCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Search teacher by email - Only Admin can access
    /// </summary>
    [HttpGet("search/email")]
    public async Task<ActionResult<TeacherDto>> GetTeacherByEmail([FromQuery] string email)
    {
        try
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email parameter is required." });
            }

            var query = new GetTeacherByEmailQuery(email);
            var teacher = await _mediator.Send(query);
            
            if (teacher == null)
            {
                return NotFound(new { message = "Teacher not found." });
            }

            return Ok(teacher);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Search teacher by employee ID - Only Admin can access
    /// </summary>
    [HttpGet("search/employee")]
    public async Task<ActionResult<TeacherDto>> GetTeacherByEmployeeId([FromQuery] string employeeId)
    {
        try
        {
            if (string.IsNullOrEmpty(employeeId))
            {
                return BadRequest(new { message = "Employee ID parameter is required." });
            }

            var query = new GetTeacherByEmployeeIdQuery(employeeId);
            var teacher = await _mediator.Send(query);
            
            if (teacher == null)
            {
                return NotFound(new { message = "Teacher not found." });
            }

            return Ok(teacher);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}