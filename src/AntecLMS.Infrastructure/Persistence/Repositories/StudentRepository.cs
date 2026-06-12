using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Infrastructure.Persistence.Repositories;

public class StudentRepository : BaseRepository<Student>, IStudentRepository
{
  public StudentRepository(AppDbContext context)
    : base(context) { }

  public async Task<Student?> GetWithUserAsync(int id, CancellationToken ct = default) =>
    await _set.Include(s => s.User)
      .Include(s => s.GroupStudents)
        .ThenInclude(gs => gs.Group)
      .FirstOrDefaultAsync(s => s.Id == id, ct);

  public async Task<(List<Student> Items, int Total)> GetPagedAsync(
    int? groupId,
    UserStatus? status,
    string? search,
    int page,
    int perPage,
    CancellationToken ct = default
  )
  {
    var query = _set.Include(s => s.User)
      .Include(s => s.GroupStudents)
        .ThenInclude(gs => gs.Group)
      .AsQueryable();

    if (groupId.HasValue)
      query = query.Where(s =>
        s.GroupStudents.Any(gs => gs.GroupId == groupId.Value && gs.Status == UserStatus.Active)
      );

    if (status.HasValue)
      query = query.Where(s => s.Status == status.Value);

    if (!string.IsNullOrEmpty(search))
      query = query.Where(s =>
        s.User.Name.Contains(search)
        || s.User.Surname.Contains(search)
        || s.User.Email.Contains(search)
      );

    var total = await query.CountAsync(ct);
    var items = await query
      .OrderByDescending(s => s.CreatedAt)
      .Skip((page - 1) * perPage)
      .Take(perPage)
      .ToListAsync(ct);

    return (items, total);
  }
}
