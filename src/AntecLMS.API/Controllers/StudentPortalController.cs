using AntecLMS.Application.DTOs;
using AntecLMS.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize(Roles = "Student")]
[Route("api/me")]
public class StudentPortalController : BaseApiController
{
  private readonly IStudentPortalService _portal;

  public StudentPortalController(IStudentPortalService portal)
  {
    _portal = portal;
  }

  [HttpGet("dashboard")]
  public async Task<IActionResult> GetMyDashboard(CancellationToken ct)
  {
    var result = await _portal.GetMyDashboardAsync(ct);
    return ToResponse(result);
  }

  [HttpGet("lessons")]
  public async Task<IActionResult> GetMyLessons(CancellationToken ct)
  {
    var result = await _portal.GetMyLessonsAsync(ct);
    return ToResponse(result);
  }

  [HttpGet("my-groups")]
  public async Task<IActionResult> GetMyGroups(CancellationToken ct)
  {
    var result = await _portal.GetMyGroupsAsync(ct);
    return ToResponse(result);
  }

  [HttpGet("attendance-journal")]
  public async Task<IActionResult> GetAttendanceJournal(
    DateTime? start,
    DateTime? end,
    CancellationToken ct
  )
  {
    var result = await _portal.GetAttendanceJournalAsync(start, end, ct);
    return ToResponse(result);
  }

  [HttpGet("profile")]
  public async Task<IActionResult> GetMyProfile(CancellationToken ct)
  {
    var result = await _portal.GetMyProfileAsync(ct);
    return ToResponse(result);
  }

  [HttpPut("change-password")]
  public async Task<IActionResult> ChangePassword(
    [FromBody] ChangePasswordRequest request,
    CancellationToken ct
  )
  {
    var result = await _portal.ChangePasswordAsync(
      request.CurrentPassword,
      request.NewPassword,
      ct
    );
    return ToResponse(result);
  }
}
