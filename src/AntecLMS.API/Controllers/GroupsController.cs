using AntecLMS.Application.DTOs;
using AntecLMS.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize]
public class GroupsController : BaseApiController
{
  private readonly IGroupService _groups;

  public GroupsController(IGroupService groups)
  {
    _groups = groups;
  }

  [HttpGet]
  public async Task<IActionResult> GetAll(
    [FromQuery] int? courseId,
    [FromQuery] int? teacherId,
    [FromQuery] string? status,
    [FromQuery] int page = 1,
    [FromQuery] int perPage = 20,
    CancellationToken ct = default
  )
  {
    var result = await _groups.GetAllAsync(courseId, teacherId, status, page, perPage, ct);
    return ToResponse(result);
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id, CancellationToken ct)
  {
    var result = await _groups.GetByIdAsync(id, ct);
    return ToResponse(result);
  }

  [HttpPost]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> Create([FromBody] CreateGroupDto dto, CancellationToken ct)
  {
    var result = await _groups.CreateAsync(dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return StatusCode(201, new { message = "Qrup uğurla yaradıldı.", data = result.Data });
  }

  [HttpPut("{id:int}")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateGroupDto dto,
    CancellationToken ct
  )
  {
    var result = await _groups.UpdateAsync(id, dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Qrup uğurla yeniləndi.", data = result.Data });
  }

  [HttpDelete("{id:int}")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> Delete(int id, CancellationToken ct)
  {
    var result = await _groups.DeleteAsync(id, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Qrup uğurla silindi." });
  }

  [HttpPost("{id:int}/students")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> AddStudent(
    int id,
    [FromBody] AddStudentToGroupDto dto,
    CancellationToken ct
  )
  {
    var result = await _groups.AddStudentAsync(id, dto.StudentId, ct);
    return ToResponse(result);
  }

  [HttpDelete("{id:int}/students/{studentId:int}")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> RemoveStudent(int id, int studentId, CancellationToken ct)
  {
    var result = await _groups.RemoveStudentAsync(id, studentId, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Tələbə qrupdan uğurla çıxarıldı." });
  }
}
