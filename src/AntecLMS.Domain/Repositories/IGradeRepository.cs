using AntecLMS.Domain.Entities;

namespace AntecLMS.Domain.Repositories;

public interface IGradeRepository : IBaseRepository<Grade>
{
  Task<List<Grade>> GetByLessonAsync(int lessonId, CancellationToken ct = default);
  Task<List<Grade>> GetByStudentAsync(int studentId, CancellationToken ct = default);
}
