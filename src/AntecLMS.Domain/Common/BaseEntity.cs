namespace AntecLMS.Domain.Common;

public abstract class BaseEntity
{
  public int Id { get; protected set; }
  public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
  public DateTime? UpdatedAt { get; protected set; }
  public DateTime? DeletedAt { get; protected set; }
  public bool IsDeleted => DeletedAt.HasValue;

  public void MarkUpdated() => UpdatedAt = DateTime.UtcNow;

  public void SoftDelete() => DeletedAt = DateTime.UtcNow;
}
