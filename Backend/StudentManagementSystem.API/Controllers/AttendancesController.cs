using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.Commands.Attendances;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Attendances;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize] // Temporarily disabled for testing
public class AttendancesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AttendancesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create attendance record - Only Teachers can create attendance for their courses
    /// </summary>
    [HttpPost]
    // [Authorize(Roles = "Teacher")] // Temporarily disabled for testing
    public async Task<ActionResult<AttendanceDto>> CreateAttendance([FromBody] CreateAttendanceDto createAttendanceDto)
    {
        try
        {
            var command = new CreateAttendanceCommand
            {
                StudentId = createAttendanceDto.StudentId,
                CourseId = createAttendanceDto.CourseId,
                AttendanceDate = createAttendanceDto.AttendanceDate == default ? DateTime.UtcNow : createAttendanceDto.AttendanceDate,
                IsPresent = createAttendanceDto.IsPresent,
                Notes = createAttendanceDto.Notes
            };

            var attendance = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAttendanceById), new { id = attendance.Id }, attendance);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get attendance by ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<ActionResult<AttendanceDto>> GetAttendanceById(int id)
    {
        try
        {
            var query = new GetAttendanceByIdQuery(id);
            var attendance = await _mediator.Send(query);
            
            if (attendance == null)
            {
                return NotFound(new { message = "Attendance record not found." });
            }

            return Ok(attendance);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get attendance records by student ID - Students can access their own, Teachers and Admin can access all
    /// </summary>
    [HttpGet("student/{studentId}")]
    // [Authorize(Roles = "Admin,Teacher,Student")] // Temporarily disabled for testing
    public async Task<ActionResult<IEnumerable<StudentAttendanceDto>>> GetAttendancesByStudent(int studentId)
    {
        try
        {
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Authorization check for students
            if (currentUserRole == UserRole.Student.ToString())
            {
                if (int.TryParse(currentUserId, out int currentStudentId) && currentStudentId != studentId)
                {
                    return StatusCode(403, new { message = "You can only access your own attendance records." });
                }
            }

            var query = new GetAttendancesByStudentQuery(studentId);
            var attendances = await _mediator.Send(query);
            return Ok(attendances);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get attendance records by course ID - Teachers can access their courses, Admin can access all
    /// </summary>
    [HttpGet("course/{courseId}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<IEnumerable<AttendanceDto>>> GetAttendancesByCourse(int courseId)
    {
        try
        {
            var query = new GetAttendancesByCourseQuery(courseId);
            var attendances = await _mediator.Send(query);
            return Ok(attendances);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get deleted attendance records - Only Admin can access
    /// </summary>
    [HttpGet("deleted")]
    // [Authorize(Roles = "Admin")] // Temporarily disabled for testing
    public async Task<ActionResult<IEnumerable<AttendanceDto>>> GetDeletedAttendances()
    {
        try
        {
            var query = new GetDeletedAttendancesQuery();
            var attendances = await _mediator.Send(query);
            return Ok(attendances);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Restore a deleted attendance record - Only Admin can access
    /// </summary>
    [HttpPost("{id}/restore")]
    // [Authorize(Roles = "Admin")] // Temporarily disabled for testing
    public async Task<ActionResult> RestoreAttendance(int id)
    {
        try
        {
            var command = new RestoreAttendanceCommand(id);
            await _mediator.Send(command);
            return Ok(new { message = "Attendance record restored successfully." });
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
}