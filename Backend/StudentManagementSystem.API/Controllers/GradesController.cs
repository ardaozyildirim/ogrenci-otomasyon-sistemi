using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.Commands.Grades;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Grades;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize] // Temporarily disabled for testing
public class GradesController : ControllerBase
{
    private readonly IMediator _mediator;

    public GradesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all grades - Only Admin can access
    /// </summary>
    [HttpGet]
    // [Authorize(Roles = "Admin")] // Temporarily disabled for testing
    public async Task<ActionResult<IEnumerable<GradeDto>>> GetAllGrades()
    {
        try
        {
            var query = new GetAllGradesQuery();
            var grades = await _mediator.Send(query);
            return Ok(grades);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get grade by ID - Admin can access any, Teacher can access grades from their courses, Student can access their own grades
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<ActionResult<GradeDto>> GetGradeById(int id)
    {
        try
        {
            var query = new GetGradeByIdQuery(id);
            var grade = await _mediator.Send(query);
            
            if (grade == null)
            {
                return NotFound(new { message = "Grade not found." });
            }

            // Authorization logic based on user role
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserRole == UserRole.Student.ToString())
            {
                // Students can only see their own grades
                if (int.TryParse(currentUserId, out int studentId) && grade.StudentId != studentId)
                {
                    return StatusCode(403, new { message = "You can only access your own grades." });
                }
            }
            else if (currentUserRole == UserRole.Teacher.ToString())
            {
                // Teachers can only see grades from their courses
                // This would require additional verification - for now, allow all teacher access
                // TODO: Add course ownership verification
            }

            return Ok(grade);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create a new grade - Only Teachers can create grades for their courses
    /// </summary>
    [HttpPost]
    // [Authorize(Roles = "Teacher")] // Temporarily disabled for testing
    public async Task<ActionResult<GradeDto>> CreateGrade([FromBody] CreateGradeDto createGradeDto)
    {
        try
        {
            var command = new CreateGradeCommand
            {
                StudentId = createGradeDto.StudentId,
                CourseId = createGradeDto.CourseId,
                Score = createGradeDto.Score,
                LetterGrade = createGradeDto.LetterGrade,
                Comments = createGradeDto.Comments,
                GradeDate = createGradeDto.GradeDate == default ? DateTime.UtcNow : createGradeDto.GradeDate,
                GradeType = createGradeDto.GradeType
            };

            var grade = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetGradeById), new { id = grade.Id }, grade);
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
    /// Update a grade - Only Teachers can update grades for their courses
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<GradeDto>> UpdateGrade(int id, [FromBody] UpdateGradeDto updateGradeDto)
    {
        try
        {
            var command = new UpdateGradeCommand
            {
                Id = id,
                Score = updateGradeDto.Score,
                LetterGrade = updateGradeDto.LetterGrade,
                Comments = updateGradeDto.Comments,
                GradeDate = updateGradeDto.GradeDate,
                GradeType = updateGradeDto.GradeType
            };

            var grade = await _mediator.Send(command);
            return Ok(grade);
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
    /// Delete a grade - Only Teachers can delete grades for their courses
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult> DeleteGrade(int id)
    {
        try
        {
            var command = new DeleteGradeCommand(id);
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
    /// Get grades by student ID - Students can access their own grades, Teachers can access grades for their courses, Admin can access all
    /// </summary>
    [HttpGet("student/{studentId}")]
    // [Authorize(Roles = "Admin,Teacher,Student")] // Temporarily disabled for testing
    public async Task<ActionResult<IEnumerable<StudentGradeDto>>> GetGradesByStudent(int studentId)
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
                    return StatusCode(403, new { message = "You can only access your own grades." });
                }
            }

            var query = new GetGradesByStudentQuery(studentId);
            var grades = await _mediator.Send(query);
            return Ok(grades);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get grades by course ID - Teachers can access grades for their courses, Admin can access all
    /// </summary>
    [HttpGet("course/{courseId}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<IEnumerable<GradeDto>>> GetGradesByCourse(int courseId)
    {
        try
        {
            var query = new GetGradesByCourseQuery(courseId);
            var grades = await _mediator.Send(query);
            return Ok(grades);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get grades by student and course - Students can access their own grades, Teachers can access for their courses, Admin can access all
    /// </summary>
    [HttpGet("student/{studentId}/course/{courseId}")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<ActionResult<IEnumerable<GradeDto>>> GetGradesByStudentAndCourse(int studentId, int courseId)
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
                    return StatusCode(403, new { message = "You can only access your own grades." });
                }
            }

            var query = new GetGradesByStudentAndCourseQuery(studentId, courseId);
            var grades = await _mediator.Send(query);
            return Ok(grades);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}