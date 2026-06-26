using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;

namespace AntecLMS.Application.Services;

public interface ITeacherDashboardService
{
  Task<Result<TeacherDashboardResponse>> GetAsync(int teacherId, CancellationToken ct);
  Task<Result<List<WeeklyScheduleItem>>> GetWeeklyScheduleAsync(int teacherId, CancellationToken ct);
}
