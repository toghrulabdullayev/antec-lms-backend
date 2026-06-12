using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Attendances.Commands.CreateAttendance;

public class CreateAttendanceHandler
  : IRequestHandler<CreateAttendanceCommand, Result<AttendanceResponse>>
{
  private readonly IAttendanceRepository _attendances;
  private readonly ILessonRepository _lessons;
  private readonly IStudentRepository _students;
  private readonly IUnitOfWork _uow;

  public CreateAttendanceHandler(
    IAttendanceRepository attendances,
    ILessonRepository lessons,
    IStudentRepository students,
    IUnitOfWork uow
  )
  {
    _attendances = attendances;
    _lessons = lessons;
    _students = students;
    _uow = uow;
  }

  public async Task<Result<AttendanceResponse>> Handle(
    CreateAttendanceCommand request,
    CancellationToken ct
  )
  {
    _ =
      await _lessons.GetByIdAsync(request.LessonId, ct)
      ?? throw new NotFoundException("Lesson", request.LessonId);
    _ =
      await _students.GetByIdAsync(request.StudentId, ct)
      ?? throw new NotFoundException("Student", request.StudentId);

    var status = Enum.Parse<AttendanceStatus>(request.Status, true);
    var attendance = Attendance.Create(
      request.LessonId,
      request.StudentId,
      status,
      request.MinutesLate,
      request.Reason,
      request.TeacherNote
    );

    await _attendances.AddAsync(attendance, ct);
    await _uow.SaveChangesAsync(ct);

    return Result<AttendanceResponse>.Success(
      new AttendanceResponse(
        attendance.Id,
        attendance.LessonId,
        attendance.StudentId,
        attendance.Status.ToString().ToLower(),
        attendance.MinutesLate,
        attendance.Reason,
        attendance.TeacherNote,
        attendance.CreatedAt
      ),
      201
    );
  }
}
