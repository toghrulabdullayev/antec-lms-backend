using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;

namespace AntecLMS.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
  Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
  Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
  Task<(List<User> Items, int Total)> GetPagedAsync(
    UserRole? role,
    UserStatus? status,
    string? search,
    int page,
    int perPage,
    CancellationToken ct = default
  );
}
