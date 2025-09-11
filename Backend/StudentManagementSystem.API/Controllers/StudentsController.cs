using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.Commands.Students;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Students;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all students - Only Admin and Teacher can access
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents()
    {
        try
        {
            var query = new GetAllStudentsQuery();
            var students = await _mediator.Send(query);
            return Ok(students);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get student by ID - Admin and Teacher can access any student, Students can only access their own data
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetStudentById(int id)
    {
        try
        {
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var currentUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            // If user is Student, they can only access their own data
            if (currentUserRole == UserRole.Student.ToString())
            {
                var currentUserQuery = new GetStudentByEmailQuery(currentUserEmail!);
                var currentStudent = await _mediator.Send(currentUserQuery);
                
                if (currentStudent == null || currentStudent.Id != id)
                {
                    return Forbid("You can only access your own student information.");
                }
            }

            var query = new GetStudentByIdQuery(id);
            var student = await _mediator.Send(query);
            
            if (student == null)
            {
                return NotFound(new { message = "Student not found." });
            }

            return Ok(student);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get current student's information - For authenticated student users
    /// </summary>
    [HttpGet("me")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<StudentDto>> GetMyInformation()
    {
        try
        {
            var currentUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            
            if (string.IsNullOrEmpty(currentUserEmail))
            {
                return BadRequest(new { message = "User email not found in token." });
            }

            var query = new GetStudentByEmailQuery(currentUserEmail);
            var student = await _mediator.Send(query);
            
            if (student == null)
            {
                return NotFound(new { message = "Student information not found." });
            }

            return Ok(student);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create a new student - Only Admin and Teacher can access
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<StudentDto>> CreateStudent([FromBody] CreateStudentDto createStudentDto)
    {
        try
        {
            var command = new CreateStudentCommand
            {
                FirstName = createStudentDto.FirstName,
                LastName = createStudentDto.LastName,
                Email = createStudentDto.Email,
                StudentNumber = createStudentDto.StudentNumber,
                PhoneNumber = createStudentDto.PhoneNumber,
                Address = createStudentDto.Address,
                DateOfBirth = createStudentDto.DateOfBirth,
                EnrollmentDate = createStudentDto.EnrollmentDate
            };

            var student = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
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
    /// Update a student - Only Admin and Teacher can access
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<StudentDto>> UpdateStudent(int id, [FromBody] UpdateStudentDto updateStudentDto)
    {
        try
        {
            var command = new UpdateStudentCommand
            {
                Id = id,
                FirstName = updateStudentDto.FirstName,
                LastName = updateStudentDto.LastName,
                Email = updateStudentDto.Email,
                StudentNumber = updateStudentDto.StudentNumber,
                PhoneNumber = updateStudentDto.PhoneNumber,
                Address = updateStudentDto.Address,
                DateOfBirth = updateStudentDto.DateOfBirth,
                EnrollmentDate = updateStudentDto.EnrollmentDate,
                IsActive = updateStudentDto.IsActive
            };

            var student = await _mediator.Send(command);
            return Ok(student);
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
    /// Delete a student - Only Admin and Teacher can access
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult> DeleteStudent(int id)
    {
        try
        {
            var command = new DeleteStudentCommand(id);
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
    /// Search student by email - Only Admin and Teacher can access
    /// </summary>
    [HttpGet("search")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<StudentDto>> GetStudentByEmail([FromQuery] string email)
    {
        try
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email parameter is required." });
            }

            var query = new GetStudentByEmailQuery(email);
            var student = await _mediator.Send(query);
            
            if (student == null)
            {
                return NotFound(new { message = "Student not found." });
            }

            return Ok(student);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}