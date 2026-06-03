using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;

namespace AntecLMS.Domain.Repositories;

public interface IStudentRepository : IBaseRepository<Student>
{
  Task<Student?> GetWithUserAsync(int id, CancellationToken ct = default);
  Task<(List<Student> Items, int Total)> GetPagedAsync(
    int? groupId,
    UserStatus? status,
    string? search,
    int page,
    int perPage,
    CancellationToken ct = default
  );
}
