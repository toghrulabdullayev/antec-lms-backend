using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Attendances.Queries.GetLessonAttendances;

public record GetLessonAttendancesQuery(int LessonId) : IRequest<Result<List<AttendanceItem>>>;

public record AttendanceItem(
  int Id,
  int StudentId,
  string? StudentName,
  string Status,
  int? MinutesLate,
  string? Reason,
  string? TeacherNote,
  DateTime CreatedAt
);
