using AntecLMS.Application.Features.Reports.Queries.GetAtRiskStudents;
using AntecLMS.Application.Features.Reports.Queries.GetAttendanceReport;
using AntecLMS.Application.Features.Reports.Queries.GetGradesReport;
using AntecLMS.Application.Features.Reports.Queries.GetGroupAttendanceStats;
using AntecLMS.Application.Features.Reports.Queries.GetStudentProgress;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize(Roles = "Admin,Teacher")]
public class ReportsController : BaseApiController
{
  [HttpGet("attendance/{groupId:int}")]
  public async Task<IActionResult> AttendanceReport(
    int groupId,
    [FromQuery] DateTime? from,
    [FromQuery] DateTime? to,
    CancellationToken ct
  )
  {
    var result = await Mediator.Send(new GetAttendanceReportQuery(groupId, from, to), ct);
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
    var result = await Mediator.Send(new GetGradesReportQuery(groupId, from, to), ct);
    return ToResponse(result);
  }

  [HttpGet("student-progress/{studentId:int}")]
  public async Task<IActionResult> StudentProgress(int studentId, CancellationToken ct)
  {
    var result = await Mediator.Send(new GetStudentProgressQuery(studentId), ct);
    return ToResponse(result);
  }

  [HttpGet("attendance-stats/{groupId:int}")]
  public async Task<IActionResult> AttendanceStats(int groupId, CancellationToken ct)
  {
    var result = await Mediator.Send(new GetGroupAttendanceStatsQuery(groupId), ct);
    return ToResponse(result);
  }

  [HttpGet("at-risk/{groupId:int}")]
  public async Task<IActionResult> AtRiskStudents(
    int groupId,
    [FromQuery] double? threshold,
    CancellationToken ct
  )
  {
    var result = await Mediator.Send(new GetAtRiskStudentsQuery(groupId, threshold), ct);
    return ToResponse(result);
  }
}
