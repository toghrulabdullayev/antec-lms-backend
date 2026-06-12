using AntecLMS.Application.DTOs;
using AntecLMS.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize(Roles = "Admin")]
public class StudentsController : BaseApiController
{
  private readonly IStudentService _students;

  public StudentsController(IStudentService students)
  {
    _students = students;
  }

  [HttpGet]
  public async Task<IActionResult> GetAll(
    [FromQuery] int? groupId,
    [FromQuery] string? status,
    [FromQuery] string? search,
    [FromQuery] int page = 1,
    [FromQuery] int perPage = 20,
    CancellationToken ct = default
  )
  {
    var result = await _students.GetAllAsync(groupId, status, search, page, perPage, ct);
    return ToResponse(result);
  }

  [HttpGet("{id:int}")]
  [Authorize(Roles = "Admin,Teacher")]
  public async Task<IActionResult> GetById(int id, CancellationToken ct)
  {
    var result = await _students.GetByIdAsync(id, ct);
    return ToResponse(result);
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateStudentDto dto, CancellationToken ct)
  {
    var result = await _students.CreateAsync(dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return StatusCode(201, new { message = "Tələbə uğurla yaradıldı.", data = result.Data });
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateStudentDto dto,
    CancellationToken ct
  )
  {
    var result = await _students.UpdateAsync(id, dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Tələbə məlumatları uğurla yeniləndi.", data = result.Data });
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id, CancellationToken ct)
  {
    var result = await _students.DeleteAsync(id, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Tələbə uğurla silindi." });
  }

  [HttpGet("{studentId:int}/attendances")]
  [Authorize(Roles = "Admin,Teacher")]
  public async Task<IActionResult> GetAttendances(int studentId, CancellationToken ct)
  {
    var result = await _students.GetAttendancesAsync(studentId, ct);
    return ToResponse(result);
  }

  [HttpGet("{studentId:int}/grades")]
  [Authorize(Roles = "Admin,Teacher")]
  public async Task<IActionResult> GetGrades(int studentId, CancellationToken ct)
  {
    var result = await _students.GetGradesAsync(studentId, ct);
    return ToResponse(result);
  }
}
