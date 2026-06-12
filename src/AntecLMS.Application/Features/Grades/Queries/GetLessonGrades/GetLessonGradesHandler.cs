using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Grades.Queries.GetLessonGrades;

public class GetLessonGradesHandler : IRequestHandler<GetLessonGradesQuery, Result<List<GradeItem>>>
{
  private readonly IGradeRepository _grades;
  private readonly ILessonRepository _lessons;

  public GetLessonGradesHandler(IGradeRepository grades, ILessonRepository lessons)
  {
    _grades = grades;
    _lessons = lessons;
  }

  public async Task<Result<List<GradeItem>>> Handle(
    GetLessonGradesQuery request,
    CancellationToken ct
  )
  {
    _ =
      await _lessons.GetByIdAsync(request.LessonId, ct)
      ?? throw new NotFoundException("Lesson", request.LessonId);

    var items = await _grades.GetByLessonAsync(request.LessonId, ct);

    var data = items
      .Select(g => new GradeItem(
        g.Id,
        g.StudentId,
        $"{g.Student?.User?.Name} {g.Student?.User?.Surname}",
        g.Score,
        g.MaxScore,
        g.TeacherNote,
        g.CreatedAt
      ))
      .ToList();

    return Result<List<GradeItem>>.Success(data);
  }
}
