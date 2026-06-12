using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Grades.Queries.GetStudentGrades;

public record GetStudentGradesQuery(int StudentId) : IRequest<Result<List<StudentGradeItem>>>;

public record StudentGradeItem(
  int Id,
  int LessonId,
  string? LessonTopic,
  DateTime LessonDate,
  int Score,
  int MaxScore,
  string? TeacherNote,
  DateTime CreatedAt
);
