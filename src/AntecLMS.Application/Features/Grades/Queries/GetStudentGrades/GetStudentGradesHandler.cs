using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Grades.Queries.GetStudentGrades;

public class GetStudentGradesHandler
  : IRequestHandler<GetStudentGradesQuery, Result<List<StudentGradeItem>>>
{
  private readonly IGradeRepository _grades;
  private readonly IStudentRepository _students;

  public GetStudentGradesHandler(IGradeRepository grades, IStudentRepository students)
  {
    _grades = grades;
    _students = students;
  }

  public async Task<Result<List<StudentGradeItem>>> Handle(
    GetStudentGradesQuery request,
    CancellationToken ct
  )
  {
    _ =
      await _students.GetByIdAsync(request.StudentId, ct)
      ?? throw new NotFoundException("Student", request.StudentId);

    var items = await _grades.GetByStudentAsync(request.StudentId, ct);

    var data = items
      .Select(g => new StudentGradeItem(
        g.Id,
        g.LessonId,
        g.Lesson?.Topic,
        g.Lesson.LessonDate,
        g.Score,
        g.MaxScore,
        g.TeacherNote,
        g.CreatedAt
      ))
      .ToList();

    return Result<List<StudentGradeItem>>.Success(data);
  }
}
