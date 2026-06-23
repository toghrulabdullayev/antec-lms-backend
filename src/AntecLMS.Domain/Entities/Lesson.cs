using AntecLMS.Domain.Common;
using AntecLMS.Domain.Enums;

namespace AntecLMS.Domain.Entities;

public class Lesson : BaseEntity
{
  public int GroupId { get; private set; }
  public int TeacherId { get; private set; }
  public DateTime LessonDate { get; private set; }
  public string? Topic { get; private set; }
  public string? Note { get; private set; }
  public LessonStatus Status { get; private set; }
  public LessonType Type { get; private set; }

  // Nav
  public Group Group { get; set; } = default!;
  public Teacher Teacher { get; set; } = default!;
  public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
  public ICollection<Grade> Grades { get; set; } = new List<Grade>();
  public ICollection<Material> Materials { get; set; } = new List<Material>();

  protected Lesson() { }

  public static Lesson Create(
    int groupId,
    int teacherId,
    DateTime lessonDate,
    string? topic,
    string? note,
    LessonStatus status,
    LessonType type
  ) =>
    new()
    {
      GroupId = groupId,
      TeacherId = teacherId,
      LessonDate = lessonDate,
      Topic = topic,
      Note = note,
      Status = status,
      Type = type,
    };

  public void Update(
    DateTime lessonDate,
    string? topic,
    string? note,
    LessonStatus status,
    LessonType type
  )
  {
    LessonDate = lessonDate;
    Topic = topic;
    Note = note;
    Status = status;
    Type = type;
    MarkUpdated();
  }
}
