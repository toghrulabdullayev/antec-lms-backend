using AntecLMS.Application.Common.Models;

namespace AntecLMS.Application.DTOs;

public record CreateGradeDto(int StudentId, int Score, int MaxScore, string? TeacherNote);

public record UpdateGradeDto(int Score, int MaxScore, string? TeacherNote);

public record GradeResponse(
  int Id,
  int LessonId,
  int StudentId,
  int Score,
  int MaxScore,
  string? TeacherNote,
  DateTime CreatedAt
);

public record GradeItem(
  int Id,
  int StudentId,
  string? StudentName,
  int Score,
  int MaxScore,
  string? TeacherNote,
  DateTime CreatedAt
);
