using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Dashboard.Queries.GetDashboard;

public record GetDashboardQuery() : IRequest<Result<DashboardResponse>>;

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
