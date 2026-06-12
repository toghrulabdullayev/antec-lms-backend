using AntecLMS.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize(Roles = "Admin,Teacher")]
public class ReportsController : BaseApiController
{
  private readonly IReportService _reports;

  public ReportsController(IReportService reports)
  {
    _reports = reports;
  }

  [HttpGet("attendance/{groupId:int}")]
  public async Task<IActionResult> AttendanceReport(
    int groupId,
    [FromQuery] DateTime? from,
    [FromQuery] DateTime? to,
    CancellationToken ct
  )
  {
    var result = await _reports.GetAttendanceReportAsync(groupId, from, to, ct);
    return ToResponse(result);
  }

  [HttpGet("grades/{groupId:int}")]
  public async Task<IActionResult> GradesReport(
    int groupId,
    [FromQuery] DateTime? from,
    [FromQuery] DateTime? to,
    CancellationToken ct
  )
  {
    var result = await _reports.GetGradesReportAsync(groupId, from, to, ct);
    return ToResponse(result);
  }

  [HttpGet("student-progress/{studentId:int}")]
  public async Task<IActionResult> StudentProgress(int studentId, CancellationToken ct)
  {
    var result = await _reports.GetStudentProgressAsync(studentId, ct);
    return ToResponse(result);
  }

  [HttpGet("attendance-stats/{groupId:int}")]
  public async Task<IActionResult> AttendanceStats(int groupId, CancellationToken ct)
  {
    var result = await _reports.GetGroupAttendanceStatsAsync(groupId, ct);
    return ToResponse(result);
  }

  [HttpGet("at-risk/{groupId:int}")]
  public async Task<IActionResult> AtRiskStudents(
    int groupId,
    [FromQuery] double? threshold,
    CancellationToken ct
  )
  {
    var result = await _reports.GetAtRiskStudentsAsync(groupId, threshold, ct);
    return ToResponse(result);
  }
}
