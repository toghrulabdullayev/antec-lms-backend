using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Attendances.Commands.UpdateAttendance;

public record UpdateAttendanceCommand(
  int Id,
  string Status,
  int? MinutesLate,
  string? Reason,
  string? TeacherNote
) : IRequest<Result<AttendanceUpdatedResponse>>;

public record AttendanceUpdatedResponse(int Id, string Status);
