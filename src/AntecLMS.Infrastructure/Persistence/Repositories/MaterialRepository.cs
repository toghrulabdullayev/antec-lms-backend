using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Infrastructure.Persistence.Repositories;

public class MaterialRepository : BaseRepository<Material>, IMaterialRepository
{
  public MaterialRepository(AppDbContext context)
    : base(context) { }

  public async Task<List<Material>> GetByGroupAsync(int groupId, CancellationToken ct = default) =>
    await _set.Where(m => m.GroupId == groupId).OrderByDescending(m => m.CreatedAt).ToListAsync(ct);

  public async Task<List<Material>> GetByLessonAsync(
    int lessonId,
    CancellationToken ct = default
  ) =>
    await _set.Where(m => m.LessonId == lessonId)
      .OrderByDescending(m => m.CreatedAt)
      .ToListAsync(ct);
  public async Task<List<Material>> GetByStudentGroupsAsync(int studentId, CancellationToken ct)
  {
    // Tələbənin qruplarını tap və o qruplara aid materialları çək
    return await _context.Materials
        .Include(m => m.Lesson)
        .Where(m => _context.GroupStudents
            .Any(sg => sg.StudentId == studentId && sg.GroupId == m.GroupId))
        .ToListAsync(ct);
  }
  public async Task<List<Material>> GetByStudentUserIdAsync(
    int userId,
    CancellationToken ct = default
  ) =>
    await _set.Include(m => m.Lesson)
      .Where(m => m.Group.GroupStudents.Any(gs => gs.Student.UserId == userId))
      .OrderByDescending(m => m.CreatedAt)
      .ToListAsync(ct);
}
