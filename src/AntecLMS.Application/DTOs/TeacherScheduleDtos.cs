namespace AntecLMS.Application.DTOs;

public record WeeklyScheduleItem(
  int GroupId,
  string GroupName,
  string DayOfWeek,
  string StartTime,
  string EndTime,
  string? RoomOrNote
);
