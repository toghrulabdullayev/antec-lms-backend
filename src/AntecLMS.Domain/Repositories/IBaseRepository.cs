using System.Linq.Expressions;
using AntecLMS.Domain.Common;

namespace AntecLMS.Domain.Repositories;

public interface IBaseRepository<T>
  where T : BaseEntity
{
  Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
  Task<List<T>> GetAllAsync(CancellationToken ct = default);
  Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
  Task AddAsync(T entity, CancellationToken ct = default);
  void Update(T entity);
  void Remove(T entity);
  IQueryable<T> GetAll();
  Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);
}
