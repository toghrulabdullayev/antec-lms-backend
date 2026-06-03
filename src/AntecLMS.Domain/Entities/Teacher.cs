using AntecLMS.Domain.Common;
using AntecLMS.Domain.Enums;

namespace AntecLMS.Domain.Entities;

public class Teacher : BaseEntity
{
  public int UserId { get; private set; }
  public string? Specialization { get; private set; }
  public string? Bio { get; private set; }
  public UserStatus Status { get; private set; }

  // Nav
  public User User { get; set; } = default!;
  public ICollection<Group> Groups { get; set; } = new List<Group>();

  protected Teacher() { }

  public static Teacher Create(
    int userId,
    string? specialization,
    string? bio,
    UserStatus status
  ) =>
    new()
    {
      UserId = userId,
      Specialization = specialization,
      Bio = bio,
      Status = status,
    };

  public void Update(string? specialization, string? bio, UserStatus status)
  {
    Specialization = specialization;
    Bio = bio;
    Status = status;
    MarkUpdated();
  }
}
