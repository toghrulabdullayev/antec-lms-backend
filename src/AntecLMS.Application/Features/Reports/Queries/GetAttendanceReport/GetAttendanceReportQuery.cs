using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Reports.Queries.GetAttendanceReport;

public record GetAttendanceReportQuery(int GroupId, DateTime? From, DateTime? To)
  : IRequest<Result<AttendanceReportResult>>;

public record AttendanceReportResult(
  int TotalLessons,
  int TotalRecords,
  int Present,
  int Absent,
  int Late,
  int Excused,
  double AttendancePercentage,
  List<AttendanceReportItem> Details
);

public record AttendanceReportItem(
  int StudentId,
  string? StudentName,
  int Present,
  int Absent,
  int Late,
  int Excused,
  double AttendancePercentage
);
