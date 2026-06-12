using AntecLMS.Application.Features.Courses.Commands.CreateCourse;
using AntecLMS.Application.Features.Courses.Commands.DeleteCourse;
using AntecLMS.Application.Features.Courses.Commands.UpdateCourse;
using AntecLMS.Application.Features.Courses.Queries.GetCourseById;
using AntecLMS.Application.Features.Courses.Queries.GetCourses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize]
public class CoursesController : BaseApiController
{
  [HttpGet]
  public async Task<IActionResult> GetAll(
    [FromQuery] string? status,
    [FromQuery] string? search,
    [FromQuery] int page = 1,
    [FromQuery] int perPage = 20,
    CancellationToken ct = default
  )
  {
    var result = await Mediator.Send(new GetCoursesQuery(status, search, page, perPage), ct);
    return ToResponse(result);
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id, CancellationToken ct)
  {
    var result = await Mediator.Send(new GetCourseByIdQuery(id), ct);
    return ToResponse(result);
  }

  [HttpPost]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> Create(
    [FromBody] CreateCourseCommand command,
    CancellationToken ct
  )
  {
    var result = await Mediator.Send(command, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return StatusCode(201, new { message = "Kurs uğurla yaradıldı.", data = result.Data });
  }

  [HttpPut("{id:int}")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateCourseRequest request,
    CancellationToken ct
  )
  {
    var result = await Mediator.Send(
      new UpdateCourseCommand(
        id,
        request.Name,
        request.Description,
        request.Price,
        request.ImageUrl,
        request.Status
      ),
      ct
    );
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Kurs uğurla yeniləndi.", data = result.Data });
  }

  [HttpDelete("{id:int}")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> Delete(int id, CancellationToken ct)
  {
    var result = await Mediator.Send(new DeleteCourseCommand(id), ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Kurs uğurla silindi." });
  }
}

public record UpdateCourseRequest(
  string Name,
  string? Description,
  decimal Price,
  string? ImageUrl,
  string Status
);
