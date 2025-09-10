using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StudentManagementSystem.Application.Commands.Attendance;
using StudentManagementSystem.Application.Queries.Attendance;
using StudentManagementSystem.Application.DTOs;
using AutoMapper;
using StudentManagementSystem.API.Models;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
[EnableRateLimiting("ApiPolicy")]
public class AttendanceController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AttendanceController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets attendance records for a specific student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Optional course ID filter</param>
    /// <returns>List of attendance records</returns>
    [HttpGet("student/{studentId}")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AttendanceDto>>), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<ActionResult<ApiResponse<IEnumerable<AttendanceDto>>>> GetStudentAttendance(
        int studentId, 
        [FromQuery] int? courseId = null)
    {
        var query = new GetStudentAttendanceQuery 
        { 
            StudentId = studentId,
            CourseId = courseId
        };

        var attendance = await _mediator.Send(query);
        return Ok(ApiResponse<IEnumerable<AttendanceDto>>.SuccessResponse(
            _mapper.Map<IEnumerable<AttendanceDto>>(attendance), 
            "Attendance records retrieved successfully"));
    }

    /// <summary>
    /// Gets attendance records for a specific course
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <param name="date">Optional date filter</param>
    /// <returns>List of attendance records</returns>
    [HttpGet("course/{courseId}")]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AttendanceDto>>), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<ActionResult<ApiResponse<IEnumerable<AttendanceDto>>>> GetCourseAttendance(
        int courseId, 
        [FromQuery] DateTime? date = null)
    {
        var query = new GetCourseAttendanceQuery 
        { 
            CourseId = courseId,
            Date = date
        };

        var attendance = await _mediator.Send(query);
        return Ok(ApiResponse<IEnumerable<AttendanceDto>>.SuccessResponse(
            _mapper.Map<IEnumerable<AttendanceDto>>(attendance), 
            "Course attendance records retrieved successfully"));
    }

    /// <summary>
    /// Records attendance for a student in a course
    /// </summary>
    /// <param name="command">Attendance record data</param>
    /// <returns>Created attendance record ID</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(typeof(ApiResponse<int>), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    public async Task<ActionResult<ApiResponse<int>>> RecordAttendance(RecordAttendanceCommand command)
    {
        var attendanceId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetStudentAttendance), 
            new { studentId = command.StudentId }, 
            ApiResponse<int>.SuccessResponse(attendanceId, "Attendance recorded successfully"));
    }

    /// <summary>
    /// Updates an attendance record
    /// </summary>
    /// <param name="id">Attendance record ID</param>
    /// <param name="command">Updated attendance data</param>
    /// <returns>No content on success</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(typeof(ApiResponse), 204)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<ActionResult<ApiResponse>> UpdateAttendance(int id, UpdateAttendanceCommand command)
    {
        if (id != command.Id)
            return BadRequest(ApiResponse.ErrorResponse("ID mismatch"));

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Gets attendance statistics for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Optional course ID filter</param>
    /// <returns>Attendance statistics</returns>
    [HttpGet("student/{studentId}/statistics")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    [ProducesResponseType(typeof(ApiResponse<AttendanceStatisticsDto>), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<ActionResult<ApiResponse<AttendanceStatisticsDto>>> GetAttendanceStatistics(
        int studentId, 
        [FromQuery] int? courseId = null)
    {
        var query = new GetAttendanceStatisticsQuery 
        { 
            StudentId = studentId,
            CourseId = courseId
        };

        var statistics = await _mediator.Send(query);
        return Ok(ApiResponse<AttendanceStatisticsDto>.SuccessResponse(
            _mapper.Map<AttendanceStatisticsDto>(statistics), 
            "Attendance statistics retrieved successfully"));
    }
}
