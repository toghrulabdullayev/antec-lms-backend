using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Application.Features.Reports.Queries.GetAttendanceReport;

public class GetAttendanceReportHandler
  : IRequestHandler<GetAttendanceReportQuery, Result<AttendanceReportResult>>
{
  private readonly IGroupRepository _groups;
  private readonly IAttendanceRepository _attendances;

  public GetAttendanceReportHandler(IGroupRepository groups, IAttendanceRepository attendances)
  {
    _groups = groups;
    _attendances = attendances;
  }

  public async Task<Result<AttendanceReportResult>> Handle(
    GetAttendanceReportQuery request,
    CancellationToken ct
  )
  {
    var group =
      await _groups.GetByIdAsync(request.GroupId, ct)
      ?? throw new NotFoundException("Group", request.GroupId);

    var query = _attendances
      .GetAll()
      .Include(a => a.Student)
        .ThenInclude(s => s.User)
      .Include(a => a.Lesson)
      .Where(a => a.Lesson.GroupId == request.GroupId);

    if (request.From.HasValue)
      query = query.Where(a => a.Lesson.LessonDate >= request.From.Value);
    if (request.To.HasValue)
      query = query.Where(a => a.Lesson.LessonDate <= request.To.Value);

    var records = await query.ToListAsync(ct);

    var byStudent = records
      .GroupBy(a => new
      {
        a.StudentId,
        Name = $"{a.Student?.User?.Name} {a.Student?.User?.Surname}",
      })
      .Select(g => new AttendanceReportItem(
        g.Key.StudentId,
        g.Key.Name,
        g.Count(a => a.Status == AttendanceStatus.Present),
        g.Count(a => a.Status == AttendanceStatus.Absent),
        g.Count(a => a.Status == AttendanceStatus.Late),
        g.Count(a => a.Status == AttendanceStatus.Excused),
        Math.Round((double)g.Count(a => a.Status != AttendanceStatus.Absent) / g.Count() * 100, 1)
      ))
      .ToList();

    var total = records.Count;
    var present = records.Count(a => a.Status == AttendanceStatus.Present);
    var absent = records.Count(a => a.Status == AttendanceStatus.Absent);
    var late = records.Count(a => a.Status == AttendanceStatus.Late);
    var excused = records.Count(a => a.Status == AttendanceStatus.Excused);

    return Result<AttendanceReportResult>.Success(
      new AttendanceReportResult(
        records.Select(a => a.LessonId).Distinct().Count(),
        total,
        present,
        absent,
        late,
        excused,
        total > 0 ? Math.Round((double)(total - absent) / total * 100, 1) : 0,
        byStudent
      )
    );
  }
}
