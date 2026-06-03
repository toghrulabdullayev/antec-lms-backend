using AntecLMS.Domain.Entities;

namespace AntecLMS.Domain.Repositories;

public interface ITeacherRepository : IBaseRepository<Teacher>
{
  Task<Teacher?> GetWithUserAsync(int id, CancellationToken ct = default);
  Task<(List<Teacher> Items, int Total)> GetPagedAsync(
    int page,
    int perPage,
    CancellationToken ct = default
  );
}
