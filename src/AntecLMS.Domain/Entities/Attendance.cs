using AntecLMS.Domain.Common;
using AntecLMS.Domain.Enums;

namespace AntecLMS.Domain.Entities;

public class Attendance : BaseEntity
{
  public int LessonId { get; private set; }
  public int StudentId { get; private set; }
  public AttendanceStatus Status { get; private set; }
  public int? MinutesLate { get; private set; }
  public string? Reason { get; private set; }
  public string? TeacherNote { get; private set; }

  // Nav
  public Lesson Lesson { get; set; } = default!;
  public Student Student { get; set; } = default!;

  protected Attendance() { }

  public static Attendance Create(
    int lessonId,
    int studentId,
    AttendanceStatus status,
    int? minutesLate,
    string? reason,
    string? teacherNote
  ) =>
    new()
    {
      LessonId = lessonId,
      StudentId = studentId,
      Status = status,
      MinutesLate = minutesLate,
      Reason = reason,
      TeacherNote = teacherNote,
    };

  public void Update(AttendanceStatus status, int? minutesLate, string? reason, string? teacherNote)
  {
    Status = status;
    MinutesLate = minutesLate;
    Reason = reason;
    TeacherNote = teacherNote;
    MarkUpdated();
  }
}
