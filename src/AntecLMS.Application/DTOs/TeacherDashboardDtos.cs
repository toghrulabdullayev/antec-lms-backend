namespace AntecLMS.Application.DTOs;

public record TeacherDashboardResponse(
  int TotalGroups,
  int TotalStudents,
  int UpcomingLessons,
  int RecentMaterials,
  int PendingGrades,
  int WeeklyLessonsCompleted,
  int WeeklyLessonsTotal,
  List<TeacherGroupItem> RecentGroups,
  List<TeacherLessonItem> RecentLessons
);

public record TeacherGroupItem(int Id, string? Name, int StudentCount);

public record TeacherLessonItem(
  int Id,
  string? GroupName,
  DateTime LessonDate,
  string Topic,
  string Status
);
