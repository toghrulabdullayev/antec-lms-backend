using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Lessons.Queries.GetLessonById;

public record GetLessonByIdQuery(int Id) : IRequest<Result<LessonDetail>>;

public record LessonDetail(
  int Id,
  int GroupId,
  string? GroupName,
  int TeacherId,
  string? TeacherName,
  DateTime LessonDate,
  string Topic,
  string Status,
  DateTime CreatedAt,
  List<LessonAttendanceItem> Attendances,
  List<LessonGradeItem> Grades
);

public record LessonAttendanceItem(
  int Id,
  int StudentId,
  string? StudentName,
  string Status,
  int? MinutesLate,
  string? Reason
);

public record LessonGradeItem(int Id, int StudentId, string? StudentName, int Score, int MaxScore);
