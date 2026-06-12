using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;

namespace AntecLMS.Domain.Repositories;

public interface IGroupRepository : IBaseRepository<Group>
{
  Task<Group?> GetWithDetailsAsync(int id, CancellationToken ct = default);
  Task<(List<Group> Items, int Total)> GetPagedAsync(
    int? courseId,
    int? teacherId,
    GroupStatus? status,
    int page,
    int perPage,
    CancellationToken ct = default
  );
  Task<bool> StudentExistsInGroupAsync(int groupId, int studentId, CancellationToken ct = default);
  Task AddStudentAsync(GroupStudent gs, CancellationToken ct = default);
  Task<GroupStudent?> GetGroupStudentAsync(
    int groupId,
    int studentId,
    CancellationToken ct = default
  );
  Task<bool> HasActiveGroupsForTeacherAsync(int teacherId, CancellationToken ct = default);
  Task<List<Group>> GetByTeacherAsync(int teacherId, CancellationToken ct = default);
}
