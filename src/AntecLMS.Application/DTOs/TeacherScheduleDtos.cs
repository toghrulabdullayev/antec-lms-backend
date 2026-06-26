namespace AntecLMS.Application.DTOs;

public record WeeklyScheduleItem(
  int LessonId,
  int GroupId,
  string GroupName,
  string Topic,
  DateTime LessonDate,
  int DayOfWeekIndex,
  int Hour
);
