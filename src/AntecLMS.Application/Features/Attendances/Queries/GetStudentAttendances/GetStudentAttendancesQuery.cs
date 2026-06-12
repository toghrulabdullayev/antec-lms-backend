using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Attendances.Queries.GetStudentAttendances;

public record GetStudentAttendancesQuery(int StudentId)
  : IRequest<Result<List<StudentAttendanceItem>>>;

public record StudentAttendanceItem(
  int Id,
  int LessonId,
  string? LessonTopic,
  DateTime LessonDate,
  string Status,
  int? MinutesLate,
  string? Reason,
  string? TeacherNote,
  DateTime CreatedAt
);
