using System.Linq.Expressions;
using AntecLMS.Domain.Common;
using AntecLMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Infrastructure.Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T>
  where T : BaseEntity
{
  protected readonly AppDbContext _context;
  protected readonly DbSet<T> _set;

  public BaseRepository(AppDbContext context)
  {
    _context = context;
    _set = context.Set<T>();
  }

  public virtual async Task<T?> GetByIdAsync(int id, CancellationToken ct = default) =>
    await _set.FirstOrDefaultAsync(e => e.Id == id, ct);

  public IQueryable<T> GetAll() => _set;

  public async Task<List<T>> GetAllAsync(CancellationToken ct = default) =>
    await _set.ToListAsync(ct);

  public async Task<List<T>> FindAsync(
    Expression<Func<T, bool>> predicate,
    CancellationToken ct = default
  ) => await _set.Where(predicate).ToListAsync(ct);

  public async Task AddAsync(T entity, CancellationToken ct = default) =>
    await _set.AddAsync(entity, ct);

  public void Update(T entity) => _set.Update(entity);

  public void Remove(T entity) => _set.Remove(entity);

  public async Task<int> CountAsync(
    Expression<Func<T, bool>>? predicate = null,
    CancellationToken ct = default
  ) => predicate is null ? await _set.CountAsync(ct) : await _set.CountAsync(predicate, ct);
}
