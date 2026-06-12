using AntecLMS.Domain.Entities;

namespace AntecLMS.Domain.Repositories;

public interface IMaterialRepository : IBaseRepository<Material>
{
  Task<List<Material>> GetByGroupAsync(int groupId, CancellationToken ct = default);
  Task<List<Material>> GetByLessonAsync(int lessonId, CancellationToken ct = default);
}
