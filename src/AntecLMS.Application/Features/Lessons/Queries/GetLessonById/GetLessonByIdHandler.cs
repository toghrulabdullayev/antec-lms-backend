using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Application.Features.Lessons.Queries.GetLessonById;

public class GetLessonByIdHandler : IRequestHandler<GetLessonByIdQuery, Result<LessonDetail>>
{
  private readonly ILessonRepository _lessons;

  public GetLessonByIdHandler(ILessonRepository lessons)
  {
    _lessons = lessons;
  }

  public async Task<Result<LessonDetail>> Handle(GetLessonByIdQuery request, CancellationToken ct)
  {
    var lesson =
      await _lessons
        .GetAll()
        .Include(l => l.Group)
        .Include(l => l.Teacher)
          .ThenInclude(t => t.User)
        .Include(l => l.Attendances)
          .ThenInclude(a => a.Student)
            .ThenInclude(s => s.User)
        .Include(l => l.Grades)
          .ThenInclude(g => g.Student)
            .ThenInclude(s => s.User)
        .FirstOrDefaultAsync(l => l.Id == request.Id, ct)
      ?? throw new NotFoundException("Lesson", request.Id);

    var detail = new LessonDetail(
      lesson.Id,
      lesson.GroupId,
      lesson.Group?.Name,
      lesson.TeacherId,
      $"{lesson.Teacher?.User?.Name} {lesson.Teacher?.User?.Surname}",
      lesson.LessonDate,
      lesson.Topic,
      lesson.Status.ToString().ToLower(),
      lesson.CreatedAt,
      lesson
        .Attendances.Select(a => new LessonAttendanceItem(
          a.Id,
          a.StudentId,
          $"{a.Student?.User?.Name} {a.Student?.User?.Surname}",
          a.Status.ToString().ToLower(),
          a.MinutesLate,
          a.Reason
        ))
        .ToList(),
      lesson
        .Grades.Select(g => new LessonGradeItem(
          g.Id,
          g.StudentId,
          $"{g.Student?.User?.Name} {g.Student?.User?.Surname}",
          g.Score,
          g.MaxScore
        ))
        .ToList()
    );

    return Result<LessonDetail>.Success(detail);
  }
}
