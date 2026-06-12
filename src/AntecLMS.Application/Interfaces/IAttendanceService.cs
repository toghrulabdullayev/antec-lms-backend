using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;

namespace AntecLMS.Application.Services;

public interface IAttendanceService
{
  Task<Result<List<AttendanceItem>>> GetByLessonAsync(int lessonId, CancellationToken ct);
  Task<Result<List<StudentAttendanceItem>>> GetByStudentAsync(int studentId, CancellationToken ct);
  Task<Result<AttendanceResponse>> CreateAsync(
    int lessonId,
    CreateAttendanceDto dto,
    CancellationToken ct
  );
  Task<Result<AttendanceResponse>> UpdateAsync(
    int id,
    UpdateAttendanceDto dto,
    CancellationToken ct
  );
  Task<Result> DeleteAsync(int id, CancellationToken ct);
}
