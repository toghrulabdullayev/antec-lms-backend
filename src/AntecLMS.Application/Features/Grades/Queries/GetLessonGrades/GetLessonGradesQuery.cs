using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Grades.Queries.GetLessonGrades;

public record GetLessonGradesQuery(int LessonId) : IRequest<Result<List<GradeItem>>>;

public record GradeItem(
  int Id,
  int StudentId,
  string? StudentName,
  int Score,
  int MaxScore,
  string? TeacherNote,
  DateTime CreatedAt
);
