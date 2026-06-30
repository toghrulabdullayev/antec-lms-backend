using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Infrastructure.Persistence.Repositories;

public class GroupScheduleRepository : BaseRepository<GroupSchedule>, IGroupScheduleRepository
{
  public GroupScheduleRepository(AppDbContext context)
    : base(context) { }

  public async Task<List<GroupSchedule>> GetByGroupAsync(int groupId, CancellationToken ct = default) =>
    await _set.Include(s => s.Group)
      .Where(s => s.GroupId == groupId)
      .OrderBy(s => s.DayOfWeek)
      .ThenBy(s => s.StartTime)
      .ToListAsync(ct);

  public async Task<List<GroupSchedule>> GetByTeacherAsync(int teacherId, CancellationToken ct = default) =>
    await _set.Include(s => s.Group)
      .Where(s => s.Group.TeacherId == teacherId)
      .OrderBy(s => s.Group.Name)
      .ThenBy(s => s.DayOfWeek)
      .ThenBy(s => s.StartTime)
      .ToListAsync(ct);

  public async Task ReplaceForGroupAsync(int groupId, List<GroupSchedule> newSchedules, CancellationToken ct = default)
  {
    var existing = await _set.Where(s => s.GroupId == groupId).ToListAsync(ct);
    _set.RemoveRange(existing);
    await _set.AddRangeAsync(newSchedules, ct);
  }
}
