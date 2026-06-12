using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;

namespace AntecLMS.Application.Services;

public interface IReportService
{
  Task<Result<AttendanceReportResult>> GetAttendanceReportAsync(
    int groupId,
    DateTime? from,
    DateTime? to,
    CancellationToken ct
  );
  Task<Result<GradesReportResult>> GetGradesReportAsync(
    int groupId,
    DateTime? from,
    DateTime? to,
    CancellationToken ct
  );
  Task<Result<StudentProgressResult>> GetStudentProgressAsync(int studentId, CancellationToken ct);
  Task<Result<GroupAttendanceStatsResult>> GetGroupAttendanceStatsAsync(
    int groupId,
    CancellationToken ct
  );
  Task<Result<List<AtRiskStudentItem>>> GetAtRiskStudentsAsync(
    int groupId,
    double? threshold,
    CancellationToken ct
  );
}
