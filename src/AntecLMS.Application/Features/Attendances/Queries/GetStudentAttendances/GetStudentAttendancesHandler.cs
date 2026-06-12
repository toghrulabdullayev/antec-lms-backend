using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Attendances.Queries.GetStudentAttendances;

public class GetStudentAttendancesHandler
  : IRequestHandler<GetStudentAttendancesQuery, Result<List<StudentAttendanceItem>>>
{
  private readonly IAttendanceRepository _attendances;
  private readonly IStudentRepository _students;

  public GetStudentAttendancesHandler(
    IAttendanceRepository attendances,
    IStudentRepository students
  )
  {
    _attendances = attendances;
    _students = students;
  }

  public async Task<Result<List<StudentAttendanceItem>>> Handle(
    GetStudentAttendancesQuery request,
    CancellationToken ct
  )
  {
    _ =
      await _students.GetByIdAsync(request.StudentId, ct)
      ?? throw new NotFoundException("Student", request.StudentId);

    var items = await _attendances.GetByStudentAsync(request.StudentId, ct);

    var data = items
      .Select(a => new StudentAttendanceItem(
        a.Id,
        a.LessonId,
        a.Lesson?.Topic,
        a.Lesson.LessonDate,
        a.Status.ToString().ToLower(),
        a.MinutesLate,
        a.Reason,
        a.TeacherNote,
        a.CreatedAt
      ))
      .ToList();

    return Result<List<StudentAttendanceItem>>.Success(data);
  }
}
