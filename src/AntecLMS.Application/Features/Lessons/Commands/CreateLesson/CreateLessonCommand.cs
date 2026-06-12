using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Lessons.Commands.CreateLesson;

public record CreateLessonCommand(
  int GroupId,
  int TeacherId,
  DateTime LessonDate,
  string Topic,
  string? Note,
  string? Status
) : IRequest<Result<LessonResponse>>;

public record LessonResponse(
  int Id,
  int GroupId,
  int TeacherId,
  DateTime LessonDate,
  string Topic,
  string Status,
  DateTime CreatedAt
);
