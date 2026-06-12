using AntecLMS.Application.Features.Dashboard.Queries.GetDashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize(Roles = "Admin")]
public class DashboardController : BaseApiController
{
  [HttpGet]
  public async Task<IActionResult> Get(CancellationToken ct)
  {
    var result = await Mediator.Send(new GetDashboardQuery(), ct);
    return ToResponse(result);
  }
}
