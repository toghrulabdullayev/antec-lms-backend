using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Infrastructure.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
  public UserRepository(AppDbContext context)
    : base(context) { }

  public override async Task<User?> GetByIdAsync(int id, CancellationToken ct = default) =>
    await _set.Include(u => u.TeacherProfile)
      .Include(u => u.StudentProfile)
      .FirstOrDefaultAsync(u => u.Id == id, ct);

  public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
    await _set.Include(u => u.TeacherProfile)
      .Include(u => u.StudentProfile)
      .FirstOrDefaultAsync(u => u.Email == email, ct);

  public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default) =>
    await _set.AnyAsync(u => u.Email == email, ct);

  public async Task<(List<User> Items, int Total)> GetPagedAsync(
    UserRole? role,
    UserStatus? status,
    string? search,
    int page,
    int perPage,
    CancellationToken ct = default
  )
  {
    var query = _set.AsQueryable();

    if (role.HasValue)
      query = query.Where(u => u.Role == role.Value);
    if (status.HasValue)
      query = query.Where(u => u.Status == status.Value);
    if (!string.IsNullOrEmpty(search))
      query = query.Where(u =>
        u.Name.Contains(search) || u.Surname.Contains(search) || u.Email.Contains(search)
      );

    var total = await query.CountAsync(ct);
    var items = await query
      .OrderByDescending(u => u.CreatedAt)
      .Skip((page - 1) * perPage)
      .Take(perPage)
      .ToListAsync(ct);

    return (items, total);
  }
}
