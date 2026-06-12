using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Application.Features.Reports.Queries.GetStudentProgress;

public class GetStudentProgressHandler
  : IRequestHandler<GetStudentProgressQuery, Result<StudentProgressResult>>
{
  private readonly IStudentRepository _students;
  private readonly IAttendanceRepository _attendances;
  private readonly IGradeRepository _grades;

  public GetStudentProgressHandler(
    IStudentRepository students,
    IAttendanceRepository attendances,
    IGradeRepository grades
  )
  {
    _students = students;
    _attendances = attendances;
    _grades = grades;
  }

  public async Task<Result<StudentProgressResult>> Handle(
    GetStudentProgressQuery request,
    CancellationToken ct
  )
  {
    var student =
      await _students
        .GetAll()
        .Include(s => s.User)
        .FirstOrDefaultAsync(s => s.Id == request.StudentId, ct)
      ?? throw new NotFoundException("Student", request.StudentId);

    var attendances = await _attendances
      .GetAll()
      .Include(a => a.Lesson)
      .Where(a => a.StudentId == request.StudentId)
      .ToListAsync(ct);

    var grades = await _grades
      .GetAll()
      .Include(g => g.Lesson)
      .Where(g => g.StudentId == request.StudentId)
      .ToListAsync(ct);

    var totalLessons = attendances.Select(a => a.LessonId).Distinct().Count();
    var attended = attendances.Count(a =>
      a.Status == Domain.Enums.AttendanceStatus.Present
      || a.Status == Domain.Enums.AttendanceStatus.Late
    );

    var recentActivity = new List<StudentProgressItem>();
    recentActivity.AddRange(
      attendances
        .OrderByDescending(a => a.CreatedAt)
        .Take(5)
        .Select(a => new StudentProgressItem(
          "attendance",
          a.Lesson?.Topic,
          a.CreatedAt,
          a.Status.ToString().ToLower()
        ))
    );
    recentActivity.AddRange(
      grades
        .OrderByDescending(g => g.CreatedAt)
        .Take(5)
        .Select(g => new StudentProgressItem(
          "grade",
          g.Lesson?.Topic,
          g.CreatedAt,
          $"{g.Score}/{g.MaxScore}"
        ))
    );

    var name = $"{student.User?.Name} {student.User?.Surname}";
    var totalScore = grades.Sum(g => g.Score);
    var totalMax = grades.Sum(g => g.MaxScore);

    return Result<StudentProgressResult>.Success(
      new StudentProgressResult(
        student.Id,
        name,
        totalLessons,
        attended,
        totalLessons > 0 ? Math.Round((double)attended / totalLessons * 100, 1) : 0,
        grades.Count,
        totalScore,
        totalMax,
        totalMax > 0 ? Math.Round((double)totalScore / totalMax * 100, 1) : 0,
        recentActivity.OrderByDescending(x => x.Date).Take(10).ToList()
      )
    );
  }
}
