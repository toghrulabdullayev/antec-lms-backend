using AntecLMS.Domain.Common;
using AntecLMS.Domain.Enums;

namespace AntecLMS.Domain.Entities;

public class Group : BaseEntity
{
  public string Name { get; private set; } = default!;
  public int CourseId { get; private set; }
  public int TeacherId { get; private set; }
  public DateOnly StartDate { get; private set; }
  public DateOnly? EndDate { get; private set; }
  public GroupStatus Status { get; private set; }

  // Nav
  public Course Course { get; set; } = default!;
  public Teacher Teacher { get; set; } = default!;
  public ICollection<GroupStudent> GroupStudents { get; set; } = new List<GroupStudent>();
  public ICollection<GroupSchedule> Schedules { get; set; } = new List<GroupSchedule>();
  public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
  public ICollection<Material> Materials { get; set; } = new List<Material>();

  protected Group() { }

  public static Group Create(
    string name,
    int courseId,
    int teacherId,
    DateOnly startDate,
    DateOnly? endDate,
    GroupStatus status
  ) =>
    new()
    {
      Name = name,
      CourseId = courseId,
      TeacherId = teacherId,
      StartDate = startDate,
      EndDate = endDate,
      Status = status,
    };

  public void Update(
    string name,
    int courseId,
    int teacherId,
    DateOnly startDate,
    DateOnly? endDate,
    GroupStatus status
  )
  {
    Name = name;
    CourseId = courseId;
    TeacherId = teacherId;
    StartDate = startDate;
    EndDate = endDate;
    Status = status;
    MarkUpdated();
  }
}
