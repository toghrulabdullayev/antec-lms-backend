using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Infrastructure.Persistence.Repositories;

public class AttendanceRepository : BaseRepository<Attendance>, IAttendanceRepository
{
  public AttendanceRepository(AppDbContext context)
    : base(context) { }

  public async Task<List<Attendance>> GetByLessonAsync(
    int lessonId,
    CancellationToken ct = default
  ) =>
    await _set.Include(a => a.Student)
        .ThenInclude(s => s.User)
      .Where(a => a.LessonId == lessonId)
      .ToListAsync(ct);

  public async Task<List<Attendance>> GetByStudentAsync(
    int studentId,
    CancellationToken ct = default
  ) =>
    await _set.Include(a => a.Lesson)
      .Where(a => a.StudentId == studentId)
      .OrderByDescending(a => a.CreatedAt)
      .ToListAsync(ct);
}
