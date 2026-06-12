using AntecLMS.Application.DTOs;
using AntecLMS.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize(Roles = "Admin")]
public class TeachersController : BaseApiController
{
  private readonly ITeacherService _teachers;

  public TeachersController(ITeacherService teachers)
  {
    _teachers = teachers;
  }

  [HttpGet]
  public async Task<IActionResult> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] int perPage = 20,
    CancellationToken ct = default
  )
  {
    var result = await _teachers.GetAllAsync(page, perPage, ct);
    return ToResponse(result);
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id, CancellationToken ct)
  {
    var result = await _teachers.GetByIdAsync(id, ct);
    return ToResponse(result);
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateTeacherDto dto, CancellationToken ct)
  {
    var result = await _teachers.CreateAsync(dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return StatusCode(201, new { message = "Müəllim uğurla yaradıldı.", data = result.Data });
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateTeacherDto dto,
    CancellationToken ct
  )
  {
    var result = await _teachers.UpdateAsync(id, dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Müəllim məlumatları uğurla yeniləndi.", data = result.Data });
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id, CancellationToken ct)
  {
    var result = await _teachers.DeleteAsync(id, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Müəllim uğurla silindi." });
  }
}
