namespace AntecLMS.Application.DTOs;

public record AttendanceReportResult(
  int TotalLessons,
  int TotalRecords,
  int Present,
  int Absent,
  int Late,
  int Excused,
  double AttendancePercentage,
  List<AttendanceReportDetail> Details
);

public record AttendanceReportDetail(
  int StudentId,
  string? StudentName,
  int Present,
  int Absent,
  int Late,
  int Excused,
  double AttendancePercentage
);

public record GradesReportResult(
  int TotalRecords,
  double AverageScore,
  double AverageMaxScore,
  double OverallPercentage,
  List<GradesReportDetail> Details
);

public record GradesReportDetail(
  int StudentId,
  string? StudentName,
  int TotalScore,
  int TotalMaxScore,
  double Percentage,
  int GradeCount
);

public record StudentProgressResult(
  int StudentId,
  string? StudentName,
  int TotalLessons,
  int AttendedLessons,
  double AttendanceRate,
  int TotalGrades,
  int TotalScore,
  int TotalMaxScore,
  double GradePercentage,
  List<ProgressActivityItem> RecentActivity
);

public record ProgressActivityItem(string Type, string? Description, DateTime Date, string? Detail);

public record GroupAttendanceStatsResult(
  int TotalStudents,
  int TotalLessons,
  double AverageAttendanceRate,
  int AtRiskCount,
  List<LessonAttendanceStat> LessonStats
);

public record LessonAttendanceStat(
  int LessonId,
  DateTime LessonDate,
  string Topic,
  int Present,
  int AbsentUnexcused,
  int Late,
  int AbsentExcused,
  double AttendanceRate
);

public record AtRiskStudentItem(
  int StudentId,
  string? StudentName,
  int TotalLessons,
  int AbsentCount,
  double AbsentRate,
  double? AverageGrade
);
