using AntecLMS.Application.Features.TeacherDashboard.Queries.GetTeacherDashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize(Roles = "Teacher")]
public class TeacherController : BaseApiController
{
  [HttpGet("dashboard/{teacherId:int}")]
  public async Task<IActionResult> Dashboard(int teacherId, CancellationToken ct)
  {
    var result = await Mediator.Send(new GetTeacherDashboardQuery(teacherId), ct);
    return ToResponse(result);
  }
}
