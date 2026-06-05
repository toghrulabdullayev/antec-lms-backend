using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Infrastructure.Persistence.Repositories;

public class CourseRepository : BaseRepository<Course>, ICourseRepository
{
  public CourseRepository(AppDbContext context)
    : base(context) { }

  public override async Task<Course?> GetByIdAsync(int id, CancellationToken ct = default) =>
    await _set.Include(c => c.Groups).FirstOrDefaultAsync(c => c.Id == id, ct);

  public async Task<(List<Course> Items, int Total)> GetPagedAsync(
    CourseStatus? status,
    string? search,
    int page,
    int perPage,
    CancellationToken ct = default
  )
  {
    var query = _set.Include(c => c.Groups).AsQueryable();

    if (status.HasValue)
      query = query.Where(c => c.Status == status.Value);

    if (!string.IsNullOrEmpty(search))
      query = query.Where(c => c.Name.Contains(search));

    var total = await query.CountAsync(ct);
    var items = await query
      .OrderByDescending(c => c.CreatedAt)
      .Skip((page - 1) * perPage)
      .Take(perPage)
      .ToListAsync(ct);

    return (items, total);
  }

  public async Task<bool> HasActiveGroupsAsync(int courseId, CancellationToken ct = default) =>
    await _context.Groups.AnyAsync(
      g => g.CourseId == courseId && g.Status == GroupStatus.Active,
      ct
    );
}
