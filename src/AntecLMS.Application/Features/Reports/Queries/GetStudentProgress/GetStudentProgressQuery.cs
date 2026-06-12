using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Reports.Queries.GetStudentProgress;

public record GetStudentProgressQuery(int StudentId) : IRequest<Result<StudentProgressResult>>;

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
  List<StudentProgressItem> RecentActivity
);

public record StudentProgressItem(string Type, string? Description, DateTime Date, string? Detail);
