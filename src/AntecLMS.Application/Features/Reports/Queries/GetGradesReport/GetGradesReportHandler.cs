using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Application.Features.Reports.Queries.GetGradesReport;

public class GetGradesReportHandler
  : IRequestHandler<GetGradesReportQuery, Result<GradesReportResult>>
{
  private readonly IGroupRepository _groups;
  private readonly IGradeRepository _grades;

  public GetGradesReportHandler(IGroupRepository groups, IGradeRepository grades)
  {
    _groups = groups;
    _grades = grades;
  }

  public async Task<Result<GradesReportResult>> Handle(
    GetGradesReportQuery request,
    CancellationToken ct
  )
  {
    var group =
      await _groups.GetByIdAsync(request.GroupId, ct)
      ?? throw new NotFoundException("Group", request.GroupId);

    var query = _grades
      .GetAll()
      .Include(g => g.Student)
        .ThenInclude(s => s.User)
      .Include(g => g.Lesson)
      .Where(g => g.Lesson.GroupId == request.GroupId);

    if (request.From.HasValue)
      query = query.Where(g => g.Lesson.LessonDate >= request.From.Value);
    if (request.To.HasValue)
      query = query.Where(g => g.Lesson.LessonDate <= request.To.Value);

    var records = await query.ToListAsync(ct);

    var byStudent = records
      .GroupBy(g => new
      {
        g.StudentId,
        Name = $"{g.Student?.User?.Name} {g.Student?.User?.Surname}",
      })
      .Select(g => new GradesReportItem(
        g.Key.StudentId,
        g.Key.Name,
        g.Sum(x => x.Score),
        g.Sum(x => x.MaxScore),
        g.Sum(x => x.MaxScore) > 0
          ? Math.Round((double)g.Sum(x => x.Score) / g.Sum(x => x.MaxScore) * 100, 1)
          : 0,
        g.Count()
      ))
      .ToList();

    return Result<GradesReportResult>.Success(
      new GradesReportResult(
        records.Count,
        records.Any() ? Math.Round(records.Average(g => g.Score), 1) : 0,
        records.Any() ? Math.Round(records.Average(g => g.MaxScore), 1) : 0,
        records.Sum(g => g.MaxScore) > 0
          ? Math.Round((double)records.Sum(g => g.Score) / records.Sum(g => g.MaxScore) * 100, 1)
          : 0,
        byStudent
      )
    );
  }
}
