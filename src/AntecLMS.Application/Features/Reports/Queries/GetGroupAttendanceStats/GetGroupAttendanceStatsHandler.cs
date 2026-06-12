using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Application.Features.Reports.Queries.GetGroupAttendanceStats;

public class GetGroupAttendanceStatsHandler
  : IRequestHandler<GetGroupAttendanceStatsQuery, Result<GroupAttendanceStatsResult>>
{
  private readonly IGroupRepository _groups;
  private readonly IAttendanceRepository _attendances;

  public GetGroupAttendanceStatsHandler(IGroupRepository groups, IAttendanceRepository attendances)
  {
    _groups = groups;
    _attendances = attendances;
  }

  public async Task<Result<GroupAttendanceStatsResult>> Handle(
    GetGroupAttendanceStatsQuery request,
    CancellationToken ct
  )
  {
    var group =
      await _groups
        .GetAll()
        .Include(g => g.GroupStudents)
          .ThenInclude(gs => gs.Student)
        .FirstOrDefaultAsync(g => g.Id == request.GroupId, ct)
      ?? throw new NotFoundException("Group", request.GroupId);

    var records = await _attendances
      .GetAll()
      .Include(a => a.Lesson)
      .Where(a => a.Lesson.GroupId == request.GroupId)
      .ToListAsync(ct);

    var lessonStats = records
      .GroupBy(a => new
      {
        a.LessonId,
        a.Lesson.LessonDate,
        a.Lesson.Topic,
      })
      .Select(g => new LessonAttendanceStat(
        g.Key.LessonId,
        g.Key.LessonDate,
        g.Key.Topic,
        g.Count(a => a.Status == Domain.Enums.AttendanceStatus.Present),
        g.Count(a => a.Status == Domain.Enums.AttendanceStatus.Absent),
        g.Count(a => a.Status == Domain.Enums.AttendanceStatus.Late),
        g.Count(a => a.Status == Domain.Enums.AttendanceStatus.Excused),
        g.Any()
          ? Math.Round(
            (double)g.Count(a => a.Status != Domain.Enums.AttendanceStatus.Absent)
              / g.Count()
              * 100,
            1
          )
          : 0
      ))
      .OrderByDescending(x => x.LessonDate)
      .ToList();

    var studentCount = group.GroupStudents.Count;
    var avgRate = lessonStats.Any() ? Math.Round(lessonStats.Average(x => x.AttendanceRate), 1) : 0;
    var atRisk = records
      .GroupBy(a => a.StudentId)
      .Count(g =>
      {
        var total = g.Count();
        var absent = g.Count(a => a.Status == Domain.Enums.AttendanceStatus.Absent);
        return total > 0 && (double)absent / total > 0.3;
      });

    return Result<GroupAttendanceStatsResult>.Success(
      new GroupAttendanceStatsResult(studentCount, lessonStats.Count, avgRate, atRisk, lessonStats)
    );
  }
}
