using AntecLMS.Domain.Common;
using AntecLMS.Domain.Enums;

namespace AntecLMS.Domain.Entities;

public class Course : BaseEntity
{
  public string Name { get; private set; } = default!;
  public string? Description { get; private set; }
  public CourseStatus Status { get; private set; }

  // Nav
  public ICollection<Group> Groups { get; set; } = new List<Group>();

  protected Course() { }

  public static Course Create(string name, string? description, CourseStatus status) =>
    new()
    {
      Name = name,
      Description = description,
      Status = status,
    };

  public void Update(string name, string? description, CourseStatus status)
  {
    Name = name;
    Description = description;
    Status = status;
    MarkUpdated();
  }
}
