using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;

namespace AntecLMS.Application.Services;

public interface ITeacherDashboardService
{
  Task<Result<TeacherDashboardResponse>> GetAsync(int teacherId, CancellationToken ct);
}
