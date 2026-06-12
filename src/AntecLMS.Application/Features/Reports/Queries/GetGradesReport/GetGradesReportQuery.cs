using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Reports.Queries.GetGradesReport;

public record GetGradesReportQuery(int GroupId, DateTime? From, DateTime? To)
  : IRequest<Result<GradesReportResult>>;

public record GradesReportResult(
  int TotalRecords,
  double AverageScore,
  double AverageMaxScore,
  double OverallPercentage,
  List<GradesReportItem> Details
);

public record GradesReportItem(
  int StudentId,
  string? StudentName,
  int TotalScore,
  int TotalMaxScore,
  double Percentage,
  int GradeCount
);
