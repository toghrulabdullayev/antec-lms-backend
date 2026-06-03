namespace AntecLMS.Domain.Entities;

public class GroupStudent
{
  public int GroupId { get; set; }
  public int StudentId { get; set; }
  public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
  public bool IsActive { get; set; } = true;

  // Nav
  public Group Group { get; set; } = default!;
  public Student Student { get; set; } = default!;
}
