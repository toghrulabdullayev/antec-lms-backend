using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Infrastructure.Persistence.Repositories;

public class GroupRepository : BaseRepository<Group>, IGroupRepository
{
  public GroupRepository(AppDbContext context)
    : base(context) { }

  public async Task<Group?> GetWithDetailsAsync(int id, CancellationToken ct = default) =>
    await _set.Include(g => g.Course)
      .Include(g => g.Teacher)
        .ThenInclude(t => t.User)
      .Include(g => g.GroupStudents)
        .ThenInclude(gs => gs.Student)
          .ThenInclude(s => s.User)
      .FirstOrDefaultAsync(g => g.Id == id, ct);

  public async Task<(List<Group> Items, int Total)> GetPagedAsync(
    int? courseId,
    int? teacherId,
    GroupStatus? status,
    int page,
    int perPage,
    CancellationToken ct = default
  )
  {
    var query = _set.Include(g => g.Course)
      .Include(g => g.Teacher)
        .ThenInclude(t => t.User)
      .Include(g => g.GroupStudents)
      .AsQueryable();

    if (courseId.HasValue)
      query = query.Where(g => g.CourseId == courseId.Value);
    if (teacherId.HasValue)
      query = query.Where(g => g.TeacherId == teacherId.Value);
    if (status.HasValue)
      query = query.Where(g => g.Status == status.Value);

    var total = await query.CountAsync(ct);
    var items = await query
      .OrderByDescending(g => g.CreatedAt)
      .Skip((page - 1) * perPage)
      .Take(perPage)
      .ToListAsync(ct);

    return (items, total);
  }

  public async Task<bool> StudentExistsInGroupAsync(
    int groupId,
    int studentId,
    CancellationToken ct = default
  ) =>
    await _context.GroupStudents.AnyAsync(
      gs => gs.GroupId == groupId && gs.StudentId == studentId && gs.Status == UserStatus.Active,
      ct
    );

  public async Task AddStudentAsync(GroupStudent gs, CancellationToken ct = default) =>
    await _context.GroupStudents.AddAsync(gs, ct);

  public async Task<GroupStudent?> GetGroupStudentAsync(
    int groupId,
    int studentId,
    CancellationToken ct = default
  ) =>
    await _context.GroupStudents.FirstOrDefaultAsync(
      gs => gs.GroupId == groupId && gs.StudentId == studentId,
      ct
    );

  public async Task<List<Group>> GetByTeacherAsync(int teacherId, CancellationToken ct = default) =>
    await _set.Include(g => g.Course)
      .Where(g => g.TeacherId == teacherId)
      .OrderByDescending(g => g.CreatedAt)
      .ToListAsync(ct);

  public async Task<bool> HasActiveGroupsForTeacherAsync(
    int teacherId,
    CancellationToken ct = default
  ) => await _set.AnyAsync(g => g.TeacherId == teacherId && g.Status == GroupStatus.Active, ct);
}
