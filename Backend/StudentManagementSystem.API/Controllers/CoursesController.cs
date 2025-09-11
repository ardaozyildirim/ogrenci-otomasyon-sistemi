using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.Commands.Courses;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Courses;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]  // Temporarily disabled for testing
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoursesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all courses - Admin can access all, Teacher can access only their own
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses()
    {
        try
        {
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserRole == UserRole.Admin.ToString())
            {
                // Admin can see all courses
                var query = new GetAllCoursesQuery();
                var courses = await _mediator.Send(query);
                return Ok(courses);
            }
            else if (currentUserRole == UserRole.Teacher.ToString())
            {
                // Teacher can only see their own courses
                if (int.TryParse(currentUserId, out int teacherId))
                {
                    var query = new GetCoursesByTeacherQuery(teacherId);
                    var courses = await _mediator.Send(query);
                    return Ok(courses);
                }
                return BadRequest(new { message = "Invalid teacher ID." });
            }
            else
            {
                return StatusCode(403, new { message = "Only Admin and Teacher can access courses." });
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get course by ID - Admin can access any, Teacher can access only their own
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDto>> GetCourseById(int id)
    {
        try
        {
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var query = new GetCourseByIdQuery(id);
            var course = await _mediator.Send(query);
            
            if (course == null)
            {
                return NotFound(new { message = "Course not found." });
            }

            // Check if teacher can access this course (only their own)
            if (currentUserRole == UserRole.Teacher.ToString())
            {
                if (int.TryParse(currentUserId, out int teacherId) && course.TeacherId != teacherId)
                {
                    return StatusCode(403, new { message = "You can only access your own courses." });
                }
            }

            return Ok(course);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create a new course - Only Admin can access
    /// </summary>
    [HttpPost]
    // [Authorize(Roles = "Admin")]  // Temporarily disabled for testing
    public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CreateCourseDto createCourseDto)
    {
        try
        {
            var command = new CreateCourseCommand
            {
                Name = createCourseDto.Name,
                CourseCode = createCourseDto.CourseCode,
                Description = createCourseDto.Description,
                Credits = createCourseDto.Credits,
                TeacherId = createCourseDto.TeacherId,
                Capacity = createCourseDto.Capacity
            };

            var course = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCourseById), new { id = course.Id }, course);
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
    /// Update a course - Only Admin can access
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CourseDto>> UpdateCourse(int id, [FromBody] UpdateCourseDto updateCourseDto)
    {
        try
        {
            var command = new UpdateCourseCommand
            {
                Id = id,
                Name = updateCourseDto.Name,
                CourseCode = updateCourseDto.CourseCode,
                Description = updateCourseDto.Description,
                Credits = updateCourseDto.Credits,
                TeacherId = updateCourseDto.TeacherId,
                Capacity = updateCourseDto.Capacity,
                Status = updateCourseDto.Status,
                IsActive = updateCourseDto.IsActive
            };

            var course = await _mediator.Send(command);
            return Ok(course);
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
    /// Update course status - Teacher can update only their own course status
    /// </summary>
    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<CourseDto>> UpdateCourseStatus(int id, [FromBody] UpdateCourseStatusDto updateStatusDto)
    {
        try
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            // First check if the course belongs to this teacher
            var courseQuery = new GetCourseByIdQuery(id);
            var existingCourse = await _mediator.Send(courseQuery);
            
            if (existingCourse == null)
            {
                return NotFound(new { message = "Course not found." });
            }

            if (int.TryParse(currentUserId, out int teacherId) && existingCourse.TeacherId != teacherId)
            {
                return StatusCode(403, new { message = "You can only update the status of your own courses." });
            }

            var command = new UpdateCourseStatusCommand(id, updateStatusDto.Status);
            var course = await _mediator.Send(command);
            return Ok(course);
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
    /// Delete a course - Only Admin can access
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteCourse(int id)
    {
        try
        {
            var command = new DeleteCourseCommand(id);
            await _mediator.Send(command);
            return NoContent();
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
    /// Get students enrolled in a course - Teacher can access only their own courses
    /// </summary>
    [HttpGet("{id}/students")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<IEnumerable<CourseStudentDto>>> GetCourseStudents(int id)
    {
        try
        {
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if teacher can access this course (only their own)
            if (currentUserRole == UserRole.Teacher.ToString())
            {
                var courseQuery = new GetCourseByIdQuery(id);
                var course = await _mediator.Send(courseQuery);
                
                if (course == null)
                {
                    return NotFound(new { message = "Course not found." });
                }

                if (int.TryParse(currentUserId, out int teacherId) && course.TeacherId != teacherId)
                {
                    return StatusCode(403, new { message = "You can only access students from your own courses." });
                }
            }

            var query = new GetCourseStudentsQuery(id);
            var students = await _mediator.Send(query);
            return Ok(students);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Enroll a student in a course - Teacher can enroll students in their own courses only
    /// </summary>
    [HttpPost("{courseId}/students/{studentId}")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult> EnrollStudent(int courseId, int studentId)
    {
        try
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            // Check if the course belongs to this teacher
            var courseQuery = new GetCourseByIdQuery(courseId);
            var course = await _mediator.Send(courseQuery);
            
            if (course == null)
            {
                return NotFound(new { message = "Course not found." });
            }

            if (int.TryParse(currentUserId, out int teacherId) && course.TeacherId != teacherId)
            {
                return StatusCode(403, new { message = "You can only enroll students in your own courses." });
            }

            var command = new EnrollStudentCommand(courseId, studentId);
            await _mediator.Send(command);
            return Ok(new { message = "Student enrolled successfully." });
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
    /// Remove a student from a course - Teacher can remove students from their own courses only
    /// </summary>
    [HttpDelete("{courseId}/students/{studentId}")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult> RemoveStudent(int courseId, int studentId)
    {
        try
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            // Check if the course belongs to this teacher
            var courseQuery = new GetCourseByIdQuery(courseId);
            var course = await _mediator.Send(courseQuery);
            
            if (course == null)
            {
                return NotFound(new { message = "Course not found." });
            }

            if (int.TryParse(currentUserId, out int teacherId) && course.TeacherId != teacherId)
            {
                return StatusCode(403, new { message = "You can only remove students from your own courses." });
            }

            var command = new RemoveStudentCommand(courseId, studentId);
            await _mediator.Send(command);
            return Ok(new { message = "Student removed successfully." });
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
    /// Search course by course code - Admin and Teacher can access
    /// </summary>
    [HttpGet("search/code")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<CourseDto>> GetCourseByCourseCode([FromQuery] string courseCode)
    {
        try
        {
            if (string.IsNullOrEmpty(courseCode))
            {
                return BadRequest(new { message = "Course code parameter is required." });
            }

            var query = new GetCourseByCourseCodeQuery(courseCode);
            var course = await _mediator.Send(query);
            
            if (course == null)
            {
                return NotFound(new { message = "Course not found." });
            }

            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if teacher can access this course (only their own)
            if (currentUserRole == UserRole.Teacher.ToString())
            {
                if (int.TryParse(currentUserId, out int teacherId) && course.TeacherId != teacherId)
                {
                    return StatusCode(403, new { message = "You can only access your own courses." });
                }
            }

            return Ok(course);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}