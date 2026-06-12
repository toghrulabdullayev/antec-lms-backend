using AntecLMS.Domain.Common;

namespace AntecLMS.Domain.Entities;

public class Material : BaseEntity
{
  public int LessonId { get; private set; }
  public int GroupId { get; private set; }
  public int TeacherId { get; private set; }
  public string Title { get; private set; } = default!;
  public string? Type { get; private set; }
  public string? Url { get; private set; }
  public string? FilePath { get; private set; }
  public string? Description { get; private set; }

  // Nav
  public Lesson Lesson { get; set; } = default!;
  public Group Group { get; set; } = default!;
  public Teacher Teacher { get; set; } = default!;

  protected Material() { }

  public static Material Create(
    int lessonId,
    int groupId,
    int teacherId,
    string title,
    string? type,
    string? url,
    string? filePath,
    string? description
  ) =>
    new()
    {
      LessonId = lessonId,
      GroupId = groupId,
      TeacherId = teacherId,
      Title = title,
      Type = type,
      Url = url,
      FilePath = filePath,
      Description = description,
    };
}
