using AntecLMS.Domain.Entities;

namespace AntecLMS.Domain.Repositories;

public interface IGroupScheduleRepository : IBaseRepository<GroupSchedule>
{
  Task<List<GroupSchedule>> GetByGroupAsync(int groupId, CancellationToken ct = default);
  Task<List<GroupSchedule>> GetByTeacherAsync(int teacherId, CancellationToken ct = default);
  Task ReplaceForGroupAsync(int groupId, List<GroupSchedule> newSchedules, CancellationToken ct = default);
}
