using AntecLMS.Application.Common.Interfaces;
using AntecLMS.Application.DTOs;
using AntecLMS.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize(Roles = "Teacher")]
public class TeacherController(
  ITeacherService teachers,
  ITeacherDashboardService dashboard,
  ICurrentUserService currentUser
) : BaseApiController
{
  [HttpGet("me")]
  public async Task<IActionResult> GetMe(CancellationToken ct)
  {
    var result = await teachers.GetMyProfileAsync(currentUser.UserId, ct);
    return ToResponse(result);
  }

  [HttpGet("dashboard/{teacherId:int}")]
  public async Task<IActionResult> Dashboard(int teacherId, CancellationToken ct)
  {
    var result = await dashboard.GetAsync(teacherId, ct);
    return ToResponse(result);
  }

  [HttpGet]
  public async Task<IActionResult> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] int perPage = 10,
    CancellationToken ct = default
  )
  {
    var result = await teachers.GetAllAsync(page, perPage, ct);
    return ToResponse(result);
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id, CancellationToken ct)
  {
    var result = await teachers.GetByIdAsync(id, ct);
    return ToResponse(result);
  }

  [HttpPost]
  public async Task<IActionResult> Create(CreateTeacherDto dto, CancellationToken ct)
  {
    var result = await teachers.CreateAsync(dto, ct);
    return ToResponse(result);
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> Update(int id, UpdateTeacherDto dto, CancellationToken ct)
  {
    var result = await teachers.UpdateAsync(id, dto, ct);
    return ToResponse(result);
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id, CancellationToken ct)
  {
    var result = await teachers.DeleteAsync(id, ct);
    return ToResponse(result);
  }

  [HttpPut("change-password")]
  public async Task<IActionResult> ChangePassword(
    [FromBody] ChangePasswordRequest request,
    CancellationToken ct
  )
  {
    var result = await teachers.ChangePasswordAsync(
      currentUser.UserId,
      request.CurrentPassword,
      request.NewPassword,
      ct
    );
    return ToResponse(result);
  }
}
