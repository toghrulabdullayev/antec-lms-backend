using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Attendances.Commands.CreateAttendance;

public record CreateAttendanceCommand(
  int LessonId,
  int StudentId,
  string Status,
  int? MinutesLate,
  string? Reason,
  string? TeacherNote
) : IRequest<Result<AttendanceResponse>>;

public record AttendanceResponse(
  int Id,
  int LessonId,
  int StudentId,
  string Status,
  int? MinutesLate,
  string? Reason,
  string? TeacherNote,
  DateTime CreatedAt
);
