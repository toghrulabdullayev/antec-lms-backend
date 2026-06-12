using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.TeacherDashboard.Queries.GetTeacherDashboard;

public record GetTeacherDashboardQuery(int TeacherId) : IRequest<Result<TeacherDashboardResult>>;

public record TeacherDashboardResult(
  int TotalGroups,
  int TotalStudents,
  int UpcomingLessons,
  int RecentMaterials,
  int PendingGrades,
  List<RecentGroupItem> RecentGroups,
  List<RecentLessonItem> RecentLessons
);

public record RecentGroupItem(int Id, string? Name, int StudentCount);

public record RecentLessonItem(
  int Id,
  string? GroupName,
  DateTime LessonDate,
  string Topic,
  string Status
);
