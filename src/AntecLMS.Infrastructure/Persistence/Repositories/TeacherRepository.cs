using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Infrastructure.Persistence.Repositories;

public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
{
  public TeacherRepository(AppDbContext context)
    : base(context) { }

  public async Task<Teacher?> GetWithUserAsync(int id, CancellationToken ct = default) =>
    await _set.Include(t => t.User).Include(t => t.Groups).FirstOrDefaultAsync(t => t.Id == id, ct);

  public async Task<Teacher?> GetByUserIdAsync(int userId, CancellationToken ct = default) =>
    await _set.Include(t => t.User).Include(t => t.Groups).FirstOrDefaultAsync(t => t.UserId == userId, ct);

  public async Task<(List<Teacher> Items, int Total)> GetPagedAsync(
    int page,
    int perPage,
    CancellationToken ct = default
  )
  {
    var query = _set.Include(t => t.User).AsQueryable();

    var total = await query.CountAsync(ct);
    var items = await query
      .OrderByDescending(t => t.CreatedAt)
      .Skip((page - 1) * perPage)
      .Take(perPage)
      .ToListAsync(ct);

    return (items, total);
  }
}
