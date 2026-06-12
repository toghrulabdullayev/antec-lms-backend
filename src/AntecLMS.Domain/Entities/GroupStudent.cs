using AntecLMS.Domain.Common;
using AntecLMS.Domain.Enums;

namespace AntecLMS.Domain.Entities;

public class GroupStudent : BaseEntity
{
  public int GroupId { get; set; }
  public int StudentId { get; set; }
  public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
  public UserStatus Status { get; set; } = UserStatus.Active;

  // Nav
  public Group Group { get; set; } = default!;
  public Student Student { get; set; } = default!;
}
