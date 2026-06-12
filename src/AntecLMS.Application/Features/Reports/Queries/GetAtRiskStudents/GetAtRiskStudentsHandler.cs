using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Application.Features.Reports.Queries.GetAtRiskStudents;

public class GetAtRiskStudentsHandler
  : IRequestHandler<GetAtRiskStudentsQuery, Result<List<AtRiskStudentItem>>>
{
  private readonly IGroupRepository _groups;
  private readonly IAttendanceRepository _attendances;
  private readonly IGradeRepository _grades;

  public GetAtRiskStudentsHandler(
    IGroupRepository groups,
    IAttendanceRepository attendances,
    IGradeRepository grades
  )
  {
    _groups = groups;
    _attendances = attendances;
    _grades = grades;
  }

  public async Task<Result<List<AtRiskStudentItem>>> Handle(
    GetAtRiskStudentsQuery request,
    CancellationToken ct
  )
  {
    var group =
      await _groups.GetByIdAsync(request.GroupId, ct)
      ?? throw new NotFoundException("Group", request.GroupId);

    var threshold = request.AbsentThreshold ?? 0.3;

    var attendances = await _attendances
      .GetAll()
      .Include(a => a.Student)
        .ThenInclude(s => s.User)
      .Include(a => a.Lesson)
      .Where(a => a.Lesson.GroupId == request.GroupId)
      .ToListAsync(ct);

    var grades = await _grades
      .GetAll()
      .Include(g => g.Lesson)
      .Where(g => g.Lesson.GroupId == request.GroupId)
      .ToListAsync(ct);

    var atRisk = attendances
      .GroupBy(a => new
      {
        a.StudentId,
        Name = $"{a.Student?.User?.Name} {a.Student?.User?.Surname}",
      })
      .Where(g =>
      {
        var total = g.Count();
        var absent = g.Count(a => a.Status == Domain.Enums.AttendanceStatus.Absent);
        return total > 0 && (double)absent / total > threshold;
      })
      .Select(g =>
      {
        var studentGrades = grades.Where(x => x.StudentId == g.Key.StudentId).ToList();
        var avgGrade = studentGrades.Any()
          ? Math.Round(
            (double)studentGrades.Sum(x => x.Score) / studentGrades.Sum(x => x.MaxScore) * 100,
            1
          )
          : (double?)null;

        return new AtRiskStudentItem(
          g.Key.StudentId,
          g.Key.Name,
          g.Count(),
          g.Count(a => a.Status == Domain.Enums.AttendanceStatus.Absent),
          Math.Round(
            (double)g.Count(a => a.Status == Domain.Enums.AttendanceStatus.Absent)
              / g.Count()
              * 100,
            1
          ),
          avgGrade
        );
      })
      .OrderByDescending(x => x.AbsentRate)
      .ToList();

    return Result<List<AtRiskStudentItem>>.Success(atRisk);
  }
}
