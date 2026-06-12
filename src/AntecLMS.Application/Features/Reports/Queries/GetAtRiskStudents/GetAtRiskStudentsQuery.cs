using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Reports.Queries.GetAtRiskStudents;

public record GetAtRiskStudentsQuery(int GroupId, double? AbsentThreshold)
  : IRequest<Result<List<AtRiskStudentItem>>>;

public record AtRiskStudentItem(
  int StudentId,
  string? StudentName,
  int TotalLessons,
  int AbsentCount,
  double AbsentRate,
  double? AverageGrade
);
