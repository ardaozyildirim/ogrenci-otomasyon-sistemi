using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.Queries.Courses;
using StudentManagementSystem.Application.DTOs;
using AutoMapper;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CoursesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
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
}
