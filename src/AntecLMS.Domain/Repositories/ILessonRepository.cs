using AntecLMS.Domain.Entities;

namespace AntecLMS.Domain.Repositories;

public interface ILessonRepository : IBaseRepository<Lesson>
{
  Task<List<Lesson>> GetByGroupAsync(int groupId, CancellationToken ct = default);
  Task<List<Lesson>> GetByTeacherAsync(int teacherId, CancellationToken ct = default);
  Task<Lesson?> GetWithDetailsAsync(int id, CancellationToken ct = default);
}
