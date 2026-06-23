using AntecLMS.Domain.Entities;

namespace AntecLMS.Domain.Repositories;

public interface IAttendanceRepository : IBaseRepository<Attendance>
{
  Task<List<Attendance>> GetByLessonAsync(int lessonId, CancellationToken ct = default);
  Task<List<Attendance>> GetByStudentAsync(int studentId, CancellationToken ct = default);
  Task<Attendance?> GetByLessonAndStudentAsync(
    int lessonId,
    int studentId,
    CancellationToken ct = default
  );
}
