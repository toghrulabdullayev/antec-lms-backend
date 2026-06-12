using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;

namespace AntecLMS.Application.Services;

public interface IDashboardService
{
  Task<Result<DashboardResponse>> GetAsync(CancellationToken ct);
}
