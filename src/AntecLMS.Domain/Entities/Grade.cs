using AntecLMS.Domain.Common;

namespace AntecLMS.Domain.Entities;

public class Grade : BaseEntity
{
  public int LessonId { get; private set; }
  public int StudentId { get; private set; }
  public int Score { get; private set; }
  public int MaxScore { get; private set; }
  public string? TeacherNote { get; private set; }

  // Nav
  public Lesson Lesson { get; set; } = default!;
  public Student Student { get; set; } = default!;

  protected Grade() { }

  public static Grade Create(
    int lessonId,
    int studentId,
    int score,
    int maxScore,
    string? teacherNote
  ) =>
    new()
    {
      LessonId = lessonId,
      StudentId = studentId,
      Score = score,
      MaxScore = maxScore,
      TeacherNote = teacherNote,
    };

  public void Update(int score, int maxScore, string? teacherNote)
  {
    Score = score;
    MaxScore = maxScore;
    TeacherNote = teacherNote;
    MarkUpdated();
  }
}
