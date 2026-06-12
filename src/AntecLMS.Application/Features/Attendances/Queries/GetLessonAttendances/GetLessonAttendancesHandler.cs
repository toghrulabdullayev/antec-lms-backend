using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Attendances.Queries.GetLessonAttendances;

public class GetLessonAttendancesHandler
  : IRequestHandler<GetLessonAttendancesQuery, Result<List<AttendanceItem>>>
{
  private readonly IAttendanceRepository _attendances;
  private readonly ILessonRepository _lessons;

  public GetLessonAttendancesHandler(IAttendanceRepository attendances, ILessonRepository lessons)
  {
    _attendances = attendances;
    _lessons = lessons;
  }

  public async Task<Result<List<AttendanceItem>>> Handle(
    GetLessonAttendancesQuery request,
    CancellationToken ct
  )
  {
    _ =
      await _lessons.GetByIdAsync(request.LessonId, ct)
      ?? throw new NotFoundException("Lesson", request.LessonId);

    var items = await _attendances.GetByLessonAsync(request.LessonId, ct);

    var data = items
      .Select(a => new AttendanceItem(
        a.Id,
        a.StudentId,
        $"{a.Student?.User?.Name} {a.Student?.User?.Surname}",
        a.Status.ToString().ToLower(),
        a.MinutesLate,
        a.Reason,
        a.TeacherNote,
        a.CreatedAt
      ))
      .ToList();

    return Result<List<AttendanceItem>>.Success(data);
  }
}
