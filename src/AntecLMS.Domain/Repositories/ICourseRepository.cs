using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;

namespace AntecLMS.Domain.Repositories;

public interface ICourseRepository : IBaseRepository<Course>
{
  Task<(List<Course> Items, int Total)> GetPagedAsync(
    CourseStatus? status,
    string? search,
    int page,
    int perPage,
    CancellationToken ct = default
  );
  Task<bool> HasActiveGroupsAsync(int courseId, CancellationToken ct = default);
}
