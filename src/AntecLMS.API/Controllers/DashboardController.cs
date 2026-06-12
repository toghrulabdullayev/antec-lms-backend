using AntecLMS.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize(Roles = "Admin")]
public class DashboardController : BaseApiController
{
  private readonly IDashboardService _dashboard;

  public DashboardController(IDashboardService dashboard)
  {
    _dashboard = dashboard;
  }

  [HttpGet]
  public async Task<IActionResult> Get(CancellationToken ct)
  {
    var result = await _dashboard.GetAsync(ct);
    return ToResponse(result);
  }
}
