using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Reports.Queries.GetGroupAttendanceStats;

public record GetGroupAttendanceStatsQuery(int GroupId)
  : IRequest<Result<GroupAttendanceStatsResult>>;

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
  int Absent,
  int Late,
  int Excused,
  double AttendanceRate
);
