namespace AntecLMS.Application.DTOs;

// Dashboard
public record MyDashboardResponse(
  MyGroupInfo? Group,
  List<MyRecentLesson> RecentLessons,
  List<MyRecentGrade> RecentGrades,
  MyAttendanceSummary AttendanceSummary,
  double FinalGrade,
  bool IsEligibleForFinal
);

public record MyGroupInfo(int Id, string Name, string Status);

public record MyRecentLesson(int Id, string Topic, DateTime LessonDate, int MaterialCount);

public record MyRecentGrade(int Id, string LessonTopic, int Score, int MaxScore);

public record MyAttendanceSummary(int Total, int Present, int Absent, int Late);

// Lessons
public record MyLessonItem(
  int Id,
  string Topic,
  string? Note,
  DateTime LessonDate,
  string GroupName,
  List<MyMaterialRef> Materials
);

public record MyMaterialRef(
  int Id,
  string Title,
  string? Description,
  string Type,
  string? Url,
  string? FilePath
);

// Attendance
public record MyAttendanceItem(
  int Id,
  DateTime LessonDate,
  string LessonTopic,
  string Status,
  int? MinutesLate,
  string? Reason
);

// Grades

// Materials
public record MyMaterialDetail(
  int Id,
  string Title,
  string? Description,
  string Type,
  string? FilePath,
  string LessonTopic,
  DateTime LessonDate
);

// Profile
public record MyProfileResponse(
  int Id,
  string Name,
  string Surname,
  string Email,
  string? Phone,
  DateOnly? BirthDate,
  string? Note,
  string Status
);

// Change Password
public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
