using AntecLMS.Application.DTOs;
using AntecLMS.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize]
public class CoursesController : BaseApiController
{
  private readonly ICourseService _courses;

  public CoursesController(ICourseService courses)
  {
    _courses = courses;
  }

  [HttpGet]
  public async Task<IActionResult> GetAll(
    [FromQuery] string? status,
    [FromQuery] string? search,
    [FromQuery] int page = 1,
    [FromQuery] int perPage = 20,
    CancellationToken ct = default
  )
  {
    var result = await _courses.GetAllAsync(status, search, page, perPage, ct);
    return ToResponse(result);
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id, CancellationToken ct)
  {
    var result = await _courses.GetByIdAsync(id, ct);
    return ToResponse(result);
  }

  [HttpPost]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> Create([FromBody] CreateCourseDto dto, CancellationToken ct)
  {
    var result = await _courses.CreateAsync(dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return StatusCode(201, new { message = "Kurs uğurla yaradıldı.", data = result.Data });
  }

  [HttpPut("{id:int}")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateCourseDto dto,
    CancellationToken ct
  )
  {
    var result = await _courses.UpdateAsync(id, dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Kurs uğurla yeniləndi.", data = result.Data });
  }

  [HttpDelete("{id:int}")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> Delete(int id, CancellationToken ct)
  {
    var result = await _courses.DeleteAsync(id, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Kurs uğurla silindi." });
  }
}
