using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Grades.Commands.CreateGrade;

public record CreateGradeCommand(
  int LessonId,
  int StudentId,
  int Score,
  int MaxScore,
  string? TeacherNote
) : IRequest<Result<GradeResponse>>;

public record GradeResponse(
  int Id,
  int LessonId,
  int StudentId,
  int Score,
  int MaxScore,
  string? TeacherNote,
  DateTime CreatedAt
);
