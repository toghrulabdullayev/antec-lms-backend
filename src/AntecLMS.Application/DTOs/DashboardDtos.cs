namespace AntecLMS.Application.DTOs;

public record DashboardResponse(
  int CourseCount,
  int GroupCount,
  int TeacherCount,
  int StudentCount,
  List<RecentGroupItem> RecentGroups,
  List<RecentLessonItem> RecentLessons
);

public record RecentGroupItem(string Name, string CourseName, bool IsActive);

public record RecentLessonItem(string Title, DateTime Date);
