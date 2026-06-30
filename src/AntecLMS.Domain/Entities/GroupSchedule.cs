using AntecLMS.Domain.Common;

namespace AntecLMS.Domain.Entities;

public class GroupSchedule : BaseEntity
{
  public int GroupId { get; private set; }
  public DayOfWeek DayOfWeek { get; private set; }
  public TimeOnly StartTime { get; private set; }
  public TimeOnly EndTime { get; private set; }
  public string? RoomOrNote { get; private set; }

  public Group Group { get; set; } = default!;

  protected GroupSchedule() { }

  public static GroupSchedule Create(int groupId, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime, string? roomOrNote) =>
    new() { GroupId = groupId, DayOfWeek = dayOfWeek, StartTime = startTime, EndTime = endTime, RoomOrNote = roomOrNote };

  public void Update(DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime, string? roomOrNote)
  {
    DayOfWeek = dayOfWeek;
    StartTime = startTime;
    EndTime = endTime;
    RoomOrNote = roomOrNote;
    MarkUpdated();
  }
}
