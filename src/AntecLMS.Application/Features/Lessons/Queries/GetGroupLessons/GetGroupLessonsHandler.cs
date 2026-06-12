using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Application.Features.Lessons.Queries.GetGroupLessons;

public class GetGroupLessonsHandler
  : IRequestHandler<GetGroupLessonsQuery, Result<List<GroupLessonItem>>>
{
  private readonly ILessonRepository _lessons;
  private readonly IGroupRepository _groups;

  public GetGroupLessonsHandler(ILessonRepository lessons, IGroupRepository groups)
  {
    _lessons = lessons;
    _groups = groups;
  }

  public async Task<Result<List<GroupLessonItem>>> Handle(
    GetGroupLessonsQuery request,
    CancellationToken ct
  )
  {
    _ =
      await _groups.GetByIdAsync(request.GroupId, ct)
      ?? throw new NotFoundException("Group", request.GroupId);

    var lessons = await _lessons
      .GetAll()
      .Where(l => l.GroupId == request.GroupId)
      .Include(l => l.Attendances)
      .Include(l => l.Grades)
      .OrderByDescending(l => l.LessonDate)
      .ToListAsync(ct);

    var data = lessons
      .Select(l => new GroupLessonItem(
        l.Id,
        l.LessonDate,
        l.Topic,
        l.Status.ToString().ToLower(),
        l.Attendances.Count,
        l.Grades.Count
      ))
      .ToList();

    return Result<List<GroupLessonItem>>.Success(data);
  }
}
