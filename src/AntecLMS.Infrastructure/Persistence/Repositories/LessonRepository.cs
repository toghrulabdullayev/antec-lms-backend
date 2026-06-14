using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Infrastructure.Persistence.Repositories;

public class LessonRepository : BaseRepository<Lesson>, ILessonRepository
{
  public LessonRepository(AppDbContext context)
    : base(context) { }

  public async Task<List<Lesson>> GetByGroupAsync(int groupId, CancellationToken ct = default) =>
    await _set.Include(l => l.Teacher)
        .ThenInclude(t => t.User)
      .Where(l => l.GroupId == groupId)
      .OrderByDescending(l => l.LessonDate)
      .ToListAsync(ct);

  public async Task<List<Lesson>> GetByTeacherAsync(
    int teacherId,
    CancellationToken ct = default
  ) =>
    await _set.Include(l => l.Group)
      .Where(l => l.TeacherId == teacherId)
      .OrderByDescending(l => l.LessonDate)
      .ToListAsync(ct);

  public async Task<List<Lesson>> GetByStudentUserIdAsync(
    int userId,
    CancellationToken ct = default
  ) =>
    await _set.Include(l => l.Group)
      .Include(l => l.Materials)
      .Where(l => l.Group.GroupStudents.Any(gs => gs.Student.UserId == userId))
      .OrderByDescending(l => l.LessonDate)
      .ToListAsync(ct);

  public async Task<Lesson?> GetWithDetailsAsync(int id, CancellationToken ct = default) =>
    await _set.Include(l => l.Group)
        .ThenInclude(g => g.Course)
      .Include(l => l.Teacher)
        .ThenInclude(t => t.User)
      .Include(l => l.Attendances)
      .Include(l => l.Grades)
      .FirstOrDefaultAsync(l => l.Id == id, ct);
}
