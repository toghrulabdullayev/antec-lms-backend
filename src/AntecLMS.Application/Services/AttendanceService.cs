using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;
using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;

namespace AntecLMS.Application.Services;

public class AttendanceService : IAttendanceService
{
  private readonly IAttendanceRepository _attendances;
  private readonly ILessonRepository _lessons;
  private readonly IStudentRepository _students;
  private readonly IUnitOfWork _uow;

  public AttendanceService(
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

  public async Task<Result<List<AttendanceItem>>> GetByLessonAsync(
    int lessonId,
    CancellationToken ct
  )
  {
    _ =
      await _lessons.GetByIdAsync(lessonId, ct) ?? throw new NotFoundException("Lesson", lessonId);

    var items = await _attendances.GetByLessonAsync(lessonId, ct);

    var data = items
      .Select(a => new AttendanceItem(
        a.Id,
        a.StudentId,
        $"{a.Student?.User?.Name} {a.Student?.User?.Surname}",
        a.Status.ToApiString(),
        a.MinutesLate,
        a.Reason,
        a.TeacherNote,
        a.CreatedAt
      ))
      .ToList();

    return Result<List<AttendanceItem>>.Success(data);
  }

  public async Task<Result<List<StudentAttendanceItem>>> GetByStudentAsync(
    int studentId,
    CancellationToken ct
  )
  {
    _ =
      await _students.GetByIdAsync(studentId, ct)
      ?? throw new NotFoundException("Student", studentId);

    var items = await _attendances.GetByStudentAsync(studentId, ct);

    var data = items
      .Select(a => new StudentAttendanceItem(
        a.Id,
        a.LessonId,
        a.Lesson?.Topic,
        a.Lesson.LessonDate,
        a.Status.ToApiString(),
        a.MinutesLate,
        a.Reason,
        a.TeacherNote,
        a.CreatedAt
      ))
      .ToList();

    return Result<List<StudentAttendanceItem>>.Success(data);
  }

  public async Task<Result<AttendanceResponse>> CreateAsync(
    int lessonId,
    CreateAttendanceDto dto,
    CancellationToken ct
)
{
    _ = await _lessons.GetByIdAsync(lessonId, ct) ?? throw new NotFoundException("Lesson", lessonId);
    _ = await _students.GetByIdAsync(dto.StudentId, ct) ?? throw new NotFoundException("Student", dto.StudentId);

    var status = dto.Status switch
    {
        "present" => AttendanceStatus.Present,
        "late" => AttendanceStatus.Late,
        "absent_excused" => AttendanceStatus.AbsentExcused,
        "absent_unexcused" => AttendanceStatus.AbsentUnexcused,
        _ => throw new ArgumentException($"Invalid status: {dto.Status}")
    };

    var existing = await _attendances.GetByLessonAndStudentAsync(lessonId, dto.StudentId, ct);

    if (existing != null)
    {
        existing.Update(status, dto.MinutesLate, dto.Reason, dto.TeacherNote);
        _attendances.Update(existing);
    }
    else
    {
        var attendance = Attendance.Create(lessonId, dto.StudentId, status, dto.MinutesLate, dto.Reason, dto.TeacherNote);
        await _attendances.AddAsync(attendance, ct);
    }

    await _uow.SaveChangesAsync(ct);

    var result = existing ?? await _attendances.GetByLessonAndStudentAsync(lessonId, dto.StudentId, ct);

    return Result<AttendanceResponse>.Success(
        new AttendanceResponse(
            result!.Id,
            result.LessonId,
            result.StudentId,
            result.Status.ToApiString(),
            result.MinutesLate,
            result.Reason,
            result.TeacherNote,
            result.CreatedAt
        ),
        201
    );
}

  public async Task<Result<AttendanceResponse>> UpdateAsync(
    int id,
    UpdateAttendanceDto dto,
    CancellationToken ct
  )
  {
    var attendance =
      await _attendances.GetByIdAsync(id, ct) ?? throw new NotFoundException("Attendance", id);

    var status = dto.Status switch
    {
        "present" => AttendanceStatus.Present,
        "late" => AttendanceStatus.Late,
        "absent_excused" => AttendanceStatus.AbsentExcused,
        "absent_unexcused" => AttendanceStatus.AbsentUnexcused,
        _ => throw new ArgumentException($"Invalid status: {dto.Status}")
    };
    attendance.Update(status, dto.MinutesLate, dto.Reason, dto.TeacherNote);

    _attendances.Update(attendance);
    await _uow.SaveChangesAsync(ct);

    return Result<AttendanceResponse>.Success(
      new AttendanceResponse(
        attendance.Id,
        attendance.LessonId,
        attendance.StudentId,
        attendance.Status.ToApiString(),
        attendance.MinutesLate,
        attendance.Reason,
        attendance.TeacherNote,
        attendance.CreatedAt
      )
    );
  }

  public async Task<Result> DeleteAsync(int id, CancellationToken ct)
  {
    var attendance =
      await _attendances.GetByIdAsync(id, ct) ?? throw new NotFoundException("Attendance", id);
    _attendances.Remove(attendance);
    await _uow.SaveChangesAsync(ct);
    return Result.Success();
  }
}
